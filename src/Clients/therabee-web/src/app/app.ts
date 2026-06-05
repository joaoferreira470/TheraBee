import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, computed, inject, signal } from '@angular/core';
import { firstValueFrom } from 'rxjs';

type View = 'overview' | 'patients' | 'sessions' | 'reports';
type AuthMode = 'login' | 'register';
type ExportFormat = 'pdf' | 'word';

type AuthUser = {
  id: string;
  name: string;
  email: string;
  role: string;
};

type Address = {
  addressLine: string;
  district: string;
  location: string;
  zipCode: string;
};

type Patient = {
  id: string;
  name: string;
  dateOfBirth: string;
  calculatedAge: number;
  patientAddress: Address;
  mainDiagnosis: string;
  gender?: string | null;
  phoneNumber?: string | null;
  email?: string | null;
  caregiverName?: string | null;
  caregiverPhone?: string | null;
  referralReason?: string | null;
  generalNotes?: string | null;
  therapistId: string;
  status: string;
};

type TherapeuticGoal = {
  id: string;
  patientId: string;
  description: string;
  area: string;
  priority: string;
  status: string;
  reviewDate?: string | null;
};

type Session = {
  id: string;
  patientId: string;
  startDateTime: string;
  endDateTime: string;
  type: string;
  location: string;
  status: string;
  clinicalSummary?: string | null;
  objectivesWorked?: string | null;
  progressRating?: string | null;
  activities?: string | null;
  patientResponse?: string | null;
  difficulties?: string | null;
  recommendations?: string | null;
  nextSteps?: string | null;
  goalIds: string[];
};

type ProgressDashboard = {
  patient: Patient;
  attendance: {
    totalSessions: number;
    completedSessions: number;
    cancelledSessions: number;
    noShowSessions: number;
    upcomingSessions: number;
    pendingRegistrationSessions: number;
    attendanceRate: number;
  };
  goals: {
    totalGoals: number;
    notStartedGoals: number;
    inProgressGoals: number;
    achievedGoals: number;
    suspendedGoals: number;
    completionRate: number;
  };
  lastSession?: Session | null;
  nextSession?: Session | null;
  generatedAt: string;
};

type Report = {
  id: string;
  patientId: string;
  therapistId: string;
  title: string;
  periodStart: string;
  periodEnd: string;
  patientSnapshot: string;
  executiveSummary: string;
  attendanceSummary: string;
  goalProgressSummary: string;
  sessionSummary: string;
  recommendations: string;
  additionalNotes?: string | null;
  hasPdfExport: boolean;
  hasWordExport: boolean;
  pdfFileName?: string | null;
  wordFileName?: string | null;
  pdfExportedAt?: string | null;
  wordExportedAt?: string | null;
  createdAt?: string | null;
};

type AuthForm = {
  name: string;
  profession: string;
  email: string;
  password: string;
};

type PatientForm = {
  name: string;
  dateOfBirth: string;
  mainDiagnosis: string;
  gender: string;
  phoneNumber: string;
  email: string;
  caregiverName: string;
  caregiverPhone: string;
  referralReason: string;
  generalNotes: string;
  addressLine: string;
  district: string;
  location: string;
  zipCode: string;
};

type GoalForm = {
  description: string;
  area: string;
  priority: string;
  reviewDate: string;
};

type SessionForm = {
  startDateTime: string;
  endDateTime: string;
  type: string;
  location: string;
};

type CheckpointForm = {
  clinicalSummary: string;
  objectivesWorked: string;
  progressRating: string;
  activities: string;
  patientResponse: string;
  difficulties: string;
  recommendations: string;
  nextSteps: string;
};

const API_BASE_URL = 'http://localhost:6001';
const TOKEN_KEY = 'therabee_token';
const USER_KEY = 'therabee_user';

@Component({
  selector: 'app-root',
  templateUrl: './app.html',
  styleUrl: './app.css',
})
export class App {
  private readonly http = inject(HttpClient);

  readonly views: { id: View; label: string; hint: string }[] = [
    { id: 'overview', label: 'Resumo', hint: 'Indicadores clinicos' },
    { id: 'patients', label: 'Paciente', hint: 'Ficha, objetivos e plano' },
    { id: 'sessions', label: 'Sessoes', hint: 'Agenda e checkpoints' },
    { id: 'reports', label: 'Relatorios', hint: 'PDF e Word' },
  ];

  readonly activeView = signal<View>('overview');
  readonly authMode = signal<AuthMode>('login');
  readonly token = signal(this.readStoredToken());
  readonly user = signal<AuthUser | null>(this.readStoredUser());
  readonly apiMessage = signal('Pronto para ligar ao backend.');
  readonly isBusy = signal(false);

  readonly patients = signal<Patient[]>([]);
  readonly sessions = signal<Session[]>([]);
  readonly goals = signal<TherapeuticGoal[]>([]);
  readonly reports = signal<Report[]>([]);
  readonly dashboard = signal<ProgressDashboard | null>(null);
  readonly selectedPatientId = signal('');
  readonly selectedSessionId = signal('');

  readonly authForm = signal<AuthForm>({
    name: 'Joana Terapeuta',
    profession: 'Psicomotricista',
    email: 'demo@therabee.local',
    password: 'Password123!',
  });

  readonly patientForm = signal<PatientForm>({
    name: 'Maria Silva',
    dateOfBirth: '1990-05-20',
    mainDiagnosis: 'Intervencao psicomotora',
    gender: 'Feminino',
    phoneNumber: '910000000',
    email: 'maria@example.com',
    caregiverName: 'Ana Silva',
    caregiverPhone: '920000000',
    referralReason: 'Acompanhamento de progresso funcional',
    generalNotes: 'Paciente acompanhada em contexto clinico.',
    addressLine: 'Rua das Flores 10',
    district: 'Lisboa',
    location: 'Lisboa',
    zipCode: '1000-001',
  });

  readonly goalForm = signal<GoalForm>({
    description: 'Melhorar coordenacao motora fina',
    area: 'Motricidade fina',
    priority: 'Medium',
    reviewDate: '2026-07-01',
  });

  readonly sessionForm = signal<SessionForm>({
    startDateTime: '2026-06-08T10:00',
    endDateTime: '2026-06-08T11:00',
    type: 'Intervention',
    location: 'Gabinete 1',
  });

  readonly checkpointForm = signal<CheckpointForm>({
    clinicalSummary: 'Sessao focada em regulacao tonica e organizacao motora.',
    objectivesWorked: 'Coordenacao bilateral; planeamento motor; atencao sustentada.',
    progressRating: 'Bom progresso',
    activities: 'Circuito motor, encaixes finos e sequencia de tarefas.',
    patientResponse: 'Boa adesao, com maior autonomia no final da sessao.',
    difficulties: 'Oscilacao atencional em tarefas longas.',
    recommendations: 'Manter rotina de exercicios curtos em casa.',
    nextSteps: 'Rever objetivos e aumentar complexidade gradualmente.',
  });

  readonly selectedPatient = computed(() => {
    return this.patients().find((patient) => patient.id === this.selectedPatientId()) ?? null;
  });

  readonly selectedPatientGoals = computed(() => {
    return this.goals().filter((goal) => goal.patientId === this.selectedPatientId());
  });

  readonly selectedPatientSessions = computed(() => {
    return this.sessions().filter((session) => session.patientId === this.selectedPatientId());
  });

  readonly selectedPatientReports = computed(() => {
    return this.reports().filter((report) => report.patientId === this.selectedPatientId());
  });

  readonly latestReport = computed(() => this.selectedPatientReports()[0] ?? null);

  readonly selectedSession = computed(() => {
    return this.sessions().find((session) => session.id === this.selectedSessionId()) ?? this.selectedPatientSessions()[0] ?? null;
  });

  readonly canUseWorkspace = computed(() => this.token().length > 0);
  readonly activePatientCount = computed(() => this.patients().filter((patient) => patient.status !== 'Archived').length);
  readonly pendingSessionCount = computed(() => {
    return this.sessions().filter((session) => session.status === 'Scheduled' || session.status === 'Rescheduled').length;
  });
  readonly completedSessionCount = computed(() => this.sessions().filter((session) => session.status === 'Completed').length);
  readonly achievedGoalCount = computed(() => this.selectedPatientGoals().filter((goal) => goal.status === 'Achieved').length);
  readonly openGoalCount = computed(() => this.selectedPatientGoals().filter((goal) => goal.status !== 'Achieved' && goal.status !== 'Suspended').length);

  constructor() {
    if (this.token()) {
      void this.loadWorkspace();
    }
  }

  selectView(view: View) {
    this.activeView.set(view);
  }

  setAuthMode(mode: AuthMode) {
    this.authMode.set(mode);
  }

  updateAuth(field: keyof AuthForm, value: string) {
    this.authForm.update((form) => ({ ...form, [field]: value }));
  }

  updatePatient(field: keyof PatientForm, value: string) {
    this.patientForm.update((form) => ({ ...form, [field]: value }));
  }

  updateGoal(field: keyof GoalForm, value: string) {
    this.goalForm.update((form) => ({ ...form, [field]: value }));
  }

  updateSession(field: keyof SessionForm, value: string) {
    this.sessionForm.update((form) => ({ ...form, [field]: value }));
  }

  updateCheckpoint(field: keyof CheckpointForm, value: string) {
    this.checkpointForm.update((form) => ({ ...form, [field]: value }));
  }

  async authenticate() {
    const form = this.authForm();
    const route = this.authMode() === 'login' ? '/auth/login' : '/auth/register';
    const payload = this.authMode() === 'login'
      ? { email: form.email, password: form.password }
      : { name: form.name, email: form.email, password: form.password, profession: form.profession };

    await this.runApi(async () => {
      const response = await firstValueFrom(this.http.post<{ auth: { accessToken: string; user: AuthUser } }>(`${API_BASE_URL}${route}`, payload));
      this.token.set(response.auth.accessToken);
      this.user.set(response.auth.user);
      localStorage.setItem(TOKEN_KEY, response.auth.accessToken);
      localStorage.setItem(USER_KEY, JSON.stringify(response.auth.user));
      await this.loadWorkspace();
      this.apiMessage.set(`Sessao iniciada como ${response.auth.user.name}.`);
    }, 'Nao foi possivel autenticar. Confirma se a API esta a correr.');
  }

  logout() {
    this.token.set('');
    this.user.set(null);
    this.patients.set([]);
    this.sessions.set([]);
    this.goals.set([]);
    this.reports.set([]);
    this.dashboard.set(null);
    this.selectedPatientId.set('');
    this.selectedSessionId.set('');
    localStorage.removeItem(TOKEN_KEY);
    localStorage.removeItem(USER_KEY);
    this.apiMessage.set('Sessao terminada localmente.');
  }

  async loadWorkspace() {
    if (!this.token()) {
      this.apiMessage.set('Faz login para carregar dados reais da API.');
      return;
    }

    await this.runApi(async () => {
      const patientsResponse = await firstValueFrom(
        this.http.get<{ patients: Patient[] }>(`${API_BASE_URL}/patients/me`, { headers: this.authHeaders() }),
      );
      this.patients.set(patientsResponse.patients);

      const firstPatientId = this.selectedPatientId() || patientsResponse.patients[0]?.id || '';
      this.selectedPatientId.set(firstPatientId);

      if (firstPatientId) {
        await this.loadPatientContext(firstPatientId);
      }

      this.apiMessage.set('Workspace sincronizado com a API.');
    }, 'Nao consegui carregar a workspace. Confirma a API, Docker e a base de dados.');
  }

  async selectPatient(patientId: string) {
    this.selectedPatientId.set(patientId);
    this.selectedSessionId.set('');
    await this.loadPatientContext(patientId);
  }

  async createPatient() {
    const form = this.patientForm();
    await this.runApi(async () => {
      await firstValueFrom(this.http.post(`${API_BASE_URL}/patients`, {
        patient: {
          name: form.name,
          dateOfBirth: this.toIsoDate(form.dateOfBirth),
          patientAddress: {
            addressLine: form.addressLine,
            district: form.district,
            location: form.location,
            zipCode: form.zipCode,
          },
          mainDiagnosis: form.mainDiagnosis,
          gender: form.gender,
          phoneNumber: form.phoneNumber,
          email: form.email,
          caregiverName: form.caregiverName,
          caregiverPhone: form.caregiverPhone,
          referralReason: form.referralReason,
          generalNotes: form.generalNotes,
        },
      }, { headers: this.authHeaders() }));

      await this.loadWorkspace();
      this.activeView.set('patients');
      this.apiMessage.set('Paciente criado e lista atualizada.');
    }, 'Nao foi possivel criar paciente.');
  }

  async createGoal() {
    const patientId = this.selectedPatientId();
    const form = this.goalForm();
    if (!patientId) {
      return;
    }

    await this.runApi(async () => {
      await firstValueFrom(this.http.post(`${API_BASE_URL}/patients/${patientId}/therapy-goals`, {
        goal: {
          description: form.description,
          area: form.area,
          priority: form.priority,
          reviewDate: form.reviewDate ? this.toIsoDate(form.reviewDate) : null,
        },
      }, { headers: this.authHeaders() }));

      await this.loadPatientContext(patientId);
      this.apiMessage.set('Objetivo terapeutico criado.');
    }, 'Nao foi possivel criar objetivo terapeutico.');
  }

  async updateGoalStatus(goalId: string, status: string) {
    const patientId = this.selectedPatientId();
    if (!patientId) {
      return;
    }

    await this.runApi(async () => {
      await firstValueFrom(this.http.patch(`${API_BASE_URL}/therapy-goals/${goalId}/status`, { status }, { headers: this.authHeaders() }));
      await this.loadPatientContext(patientId);
      this.apiMessage.set('Estado do objetivo atualizado.');
    }, 'Nao foi possivel atualizar o estado do objetivo.');
  }

  async createSession() {
    const patientId = this.selectedPatientId();
    const form = this.sessionForm();
    if (!patientId) {
      return;
    }

    await this.runApi(async () => {
      await firstValueFrom(this.http.post(`${API_BASE_URL}/patients/${patientId}/sessions`, {
        session: {
          startDateTime: this.toIsoDateTime(form.startDateTime),
          endDateTime: this.toIsoDateTime(form.endDateTime),
          type: form.type,
          location: form.location,
          goalIds: this.selectedPatientGoals().map((goal) => goal.id),
        },
      }, { headers: this.authHeaders() }));

      await this.loadPatientContext(patientId);
      this.activeView.set('sessions');
      this.apiMessage.set('Sessao agendada.');
    }, 'Nao foi possivel agendar sessao.');
  }

  startSchedulingForSelectedPatient() {
    this.activeView.set('sessions');
  }

  async completeSession(sessionId: string) {
    const patientId = this.selectedPatientId();
    const form = this.checkpointForm();
    if (!patientId) {
      return;
    }

    await this.runApi(async () => {
      await firstValueFrom(this.http.patch(`${API_BASE_URL}/sessions/${sessionId}/complete`, { session: form }, { headers: this.authHeaders() }));
      await this.loadPatientContext(patientId);
      this.apiMessage.set('Checkpoint registado e sessao concluida.');
    }, 'Nao foi possivel concluir a sessao.');
  }

  async createReportDraft() {
    const patientId = this.selectedPatientId();
    if (!patientId) {
      return;
    }

    await this.runApi(async () => {
      await firstValueFrom(this.http.post(`${API_BASE_URL}/patients/${patientId}/reports/draft`, {}, { headers: this.authHeaders() }));
      await this.loadPatientContext(patientId);
      this.activeView.set('reports');
      this.apiMessage.set('Draft de relatorio gerado.');
    }, 'Nao foi possivel gerar o draft de relatorio.');
  }

  async exportReport(reportId: string, format: ExportFormat) {
    await this.runApi(async () => {
      const response = await firstValueFrom(this.http.post(`${API_BASE_URL}/reports/${reportId}/export/${format}`, {}, {
        headers: this.authHeaders(),
        observe: 'response',
        responseType: 'blob',
      }));

      const blob = response.body;
      if (!blob) {
        return;
      }

      const fileName = this.extractFileName(response.headers.get('content-disposition')) ?? `therabee-report.${format === 'pdf' ? 'pdf' : 'docx'}`;
      const url = URL.createObjectURL(blob);
      const anchor = document.createElement('a');
      anchor.href = url;
      anchor.download = fileName;
      anchor.click();
      URL.revokeObjectURL(url);
      this.apiMessage.set(`Relatorio exportado em ${format.toUpperCase()}.`);
    }, `Nao foi possivel exportar em ${format.toUpperCase()}.`);
  }

  async discardReport(reportId: string) {
    const patientId = this.selectedPatientId();
    if (!patientId) {
      return;
    }

    await this.runApi(async () => {
      await firstValueFrom(this.http.delete(`${API_BASE_URL}/reports/${reportId}`, { headers: this.authHeaders() }));
      await this.loadPatientContext(patientId);
      this.apiMessage.set('Rascunho de relatorio descartado.');
    }, 'Nao foi possivel descartar o rascunho.');
  }

  selectSession(sessionId: string) {
    this.selectedSessionId.set(sessionId);
  }

  patientAddress(patient: Patient) {
    return [
      patient.patientAddress?.addressLine,
      patient.patientAddress?.zipCode,
      patient.patientAddress?.location,
      patient.patientAddress?.district,
    ].filter(Boolean).join(', ');
  }

  formatDate(value?: string | null) {
    if (!value) {
      return 'Sem data';
    }

    return new Intl.DateTimeFormat('pt-PT', { dateStyle: 'medium' }).format(new Date(value));
  }

  formatDateTime(value?: string | null) {
    if (!value) {
      return 'Sem data';
    }

    return new Intl.DateTimeFormat('pt-PT', { dateStyle: 'medium', timeStyle: 'short' }).format(new Date(value));
  }

  goalStatusLabel(status: string) {
    return {
      NotStarted: 'Nao iniciado',
      InProgress: 'Em progresso',
      Achieved: 'Alcancado',
      Suspended: 'Suspenso',
    }[status] ?? status;
  }

  goalPriorityLabel(priority: string) {
    return {
      Low: 'Baixa',
      Medium: 'Media',
      High: 'Alta',
    }[priority] ?? priority;
  }

  sessionStatusLabel(status: string) {
    return {
      Scheduled: 'Agendada',
      Rescheduled: 'Reagendada',
      Completed: 'Concluida',
      Cancelled: 'Cancelada',
      NoShow: 'Falta',
    }[status] ?? status;
  }

  sessionTypeLabel(type: string) {
    return {
      Assessment: 'Avaliacao',
      Intervention: 'Terapia',
      Reassessment: 'Reavaliacao',
    }[type] ?? type;
  }

  trackById(_: number, item: { id: string }) {
    return item.id;
  }

  private async loadPatientContext(patientId: string) {
    if (!patientId || !this.token()) {
      return;
    }

    const headers = this.authHeaders();
    const [sessionsResponse, goalsResponse, reportsResponse, dashboardResponse] = await Promise.all([
      firstValueFrom(this.http.get<{ sessions: Session[] }>(`${API_BASE_URL}/patients/${patientId}/sessions`, { headers })),
      firstValueFrom(this.http.get<{ therapeuticGoals: TherapeuticGoal[] }>(`${API_BASE_URL}/patients/${patientId}/therapy-goals`, { headers })),
      firstValueFrom(this.http.get<{ reports: Report[] }>(`${API_BASE_URL}/patients/${patientId}/reports`, { headers })),
      firstValueFrom(this.http.get<{ dashboard: ProgressDashboard }>(`${API_BASE_URL}/patients/${patientId}/progress-dashboard`, { headers })),
    ]);

    this.sessions.update((sessions) => [
      ...sessions.filter((session) => session.patientId !== patientId),
      ...sessionsResponse.sessions,
    ]);
    this.goals.update((goals) => [
      ...goals.filter((goal) => goal.patientId !== patientId),
      ...goalsResponse.therapeuticGoals,
    ]);
    this.reports.update((reports) => [
      ...reports.filter((report) => report.patientId !== patientId),
      ...reportsResponse.reports,
    ]);
    this.dashboard.set(dashboardResponse.dashboard);

    const firstSessionId = sessionsResponse.sessions[0]?.id || '';
    this.selectedSessionId.set(this.selectedSessionId() || firstSessionId);
  }

  private authHeaders() {
    return new HttpHeaders({ Authorization: `Bearer ${this.token()}` });
  }

  private async runApi(action: () => Promise<void>, failureMessage: string) {
    this.isBusy.set(true);
    try {
      await action();
    } catch (error) {
      console.error(error);
      this.apiMessage.set(failureMessage);
    } finally {
      this.isBusy.set(false);
    }
  }

  private readStoredToken() {
    return localStorage.getItem(TOKEN_KEY) ?? '';
  }

  private readStoredUser() {
    const userJson = localStorage.getItem(USER_KEY);
    if (!userJson) {
      return null;
    }

    try {
      return JSON.parse(userJson) as AuthUser;
    } catch {
      return null;
    }
  }

  private toIsoDate(value: string) {
    return new Date(`${value}T00:00:00`).toISOString();
  }

  private toIsoDateTime(value: string) {
    return new Date(value).toISOString();
  }

  private extractFileName(disposition: string | null) {
    const match = disposition?.match(/filename="?([^"]+)"?/i);
    return match?.[1];
  }
}

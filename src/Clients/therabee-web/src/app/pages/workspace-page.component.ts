import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, computed, effect, inject, signal } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { firstValueFrom } from 'rxjs';
import { toSignal } from '@angular/core/rxjs-interop';

type WorkspacePage = 'therapist' | 'patient' | 'session';

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
  type: string;
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

type PatientEditForm = {
  id: string;
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
  type: string;
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
  goalIds: string[];
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
  selector: 'app-workspace-page',
  standalone: true,
  templateUrl: './workspace-page.component.html',
  styleUrl: './workspace-page.component.css',
})
export class WorkspacePageComponent {
  private readonly http = inject(HttpClient);
  private readonly router = inject(Router);
  private readonly route = inject(ActivatedRoute);
  private readonly routeData = toSignal(this.route.data, { initialValue: this.route.snapshot.data });
  private readonly routeParams = toSignal(this.route.paramMap, { initialValue: this.route.snapshot.paramMap });

  readonly currentPage = computed<WorkspacePage>(() => (this.routeData()?.['page'] as WorkspacePage) ?? 'therapist');
  readonly token = signal(this.readStoredToken());
  readonly user = signal<AuthUser | null>(this.readStoredUser());
  readonly apiMessage = signal('Workspace pronto para sincronizar.');
  readonly isBusy = signal(false);

  readonly patients = signal<Patient[]>([]);
  readonly sessions = signal<Session[]>([]);
  readonly goals = signal<TherapeuticGoal[]>([]);
  readonly reports = signal<Report[]>([]);
  readonly dashboard = signal<ProgressDashboard | null>(null);
  readonly selectedPatientId = signal('');
  readonly selectedSessionId = signal('');
  readonly selectedPatientDetail = signal<Patient | null>(null);
  readonly selectedSessionDetail = signal<Session | null>(null);
  readonly editingGoalId = signal('');
  readonly editingPatientId = signal('');

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

  readonly patientEditForm = signal<PatientEditForm>({
    id: '',
    name: '',
    dateOfBirth: '',
    mainDiagnosis: '',
    gender: '',
    phoneNumber: '',
    email: '',
    caregiverName: '',
    caregiverPhone: '',
    referralReason: '',
    generalNotes: '',
    addressLine: '',
    district: '',
    location: '',
    zipCode: '',
  });

  readonly goalForm = signal<GoalForm>({
    type: 'Objective',
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
    goalIds: [],
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

  readonly selectedPatient = computed(() => this.selectedPatientDetail() ?? this.patients().find((patient) => patient.id === this.selectedPatientId()) ?? null);
  readonly selectedSession = computed(() => this.selectedSessionDetail() ?? this.sessions().find((session) => session.id === this.selectedSessionId()) ?? null);
  readonly selectedPatientGoals = computed(() => this.goals().filter((goal) => goal.patientId === this.selectedPatientId()));
  readonly selectedPatientAreas = computed(() => this.selectedPatientGoals().filter((goal) => goal.type === 'Area'));
  readonly selectedPatientObjectives = computed(() => this.selectedPatientGoals().filter((goal) => goal.type === 'Objective'));
  readonly availableSessionGoals = computed(() => this.sessionForm().type === 'Intervention'
    ? this.selectedPatientObjectives()
    : this.selectedPatientAreas());
  readonly selectedPatientSessions = computed(() => this.sessions().filter((session) => session.patientId === this.selectedPatientId()).sort((a, b) => new Date(a.startDateTime).getTime() - new Date(b.startDateTime).getTime()));
  readonly selectedPatientReports = computed(() => this.reports().filter((report) => report.patientId === this.selectedPatientId()).sort((a, b) => new Date(b.createdAt ?? 0).getTime() - new Date(a.createdAt ?? 0).getTime()));
  readonly allSessions = computed(() => this.sessions().slice().sort((a, b) => new Date(a.startDateTime).getTime() - new Date(b.startDateTime).getTime()));
  readonly upcomingSessions = computed(() => this.allSessions().filter((session) => session.status === 'Scheduled' || session.status === 'Rescheduled'));
  readonly therapistName = computed(() => this.user()?.name || 'Terapeuta');
  readonly currentPatientLabel = computed(() => {
    const patient = this.selectedPatient();
    return patient ? `Paciente ${patient.name}` : 'Paciente';
  });
  readonly currentSessionLabel = computed(() => {
    const patient = this.selectedPatient();
    const session = this.selectedSession();
    if (!patient || !session) {
      return 'Sessao';
    }

    const sessionNumber = Math.max(this.selectedPatientSessions().findIndex((item) => item.id === session.id) + 1, 1);
    return `Sessao ${sessionNumber} | ${patient.name}, ${patient.calculatedAge} anos`;
  });

  readonly activePatientCount = computed(() => this.patients().filter((patient) => patient.status !== 'Archived').length);
  readonly pendingSessionCount = computed(() => this.sessions().filter((session) => session.status === 'Scheduled' || session.status === 'Rescheduled').length);
  readonly completedSessionCount = computed(() => this.sessions().filter((session) => session.status === 'Completed').length);
  readonly achievedGoalCount = computed(() => this.selectedPatientGoals().filter((goal) => goal.status === 'Achieved').length);
  readonly openGoalCount = computed(() => this.selectedPatientGoals().filter((goal) => goal.status !== 'Achieved' && goal.status !== 'Suspended').length);
  readonly patientAddressLine = computed(() => this.selectedPatient() ? this.patientAddress(this.selectedPatient()!) : '');

  constructor() {
    if (!this.token()) {
      void this.router.navigate(['/login']);
      return;
    }

    void this.loadWorkspace();

    effect(() => {
      const patientId = this.routeParams().get('patientId') ?? '';
      const sessionId = this.routeParams().get('sessionId') ?? '';
      this.selectedPatientId.set(patientId);
      this.selectedSessionId.set(sessionId);

      if (patientId) {
        void this.loadPatientContext(patientId, sessionId);
      } else {
        this.selectedPatientDetail.set(null);
        this.selectedSessionDetail.set(null);
        this.dashboard.set(null);
      }
    });
  }

  async goTherapist() {
    await this.router.navigate(['/therapist']);
  }

  async goPatient(patientId: string) {
    await this.router.navigate(['/patients', patientId]);
  }

  async goSession(patientId: string, sessionId: string) {
    await this.router.navigate(['/patients', patientId, 'sessions', sessionId]);
  }

  async logout() {
    this.token.set('');
    this.user.set(null);
    this.patients.set([]);
    this.sessions.set([]);
    this.goals.set([]);
    this.reports.set([]);
    this.dashboard.set(null);
    this.selectedPatientId.set('');
    this.selectedSessionId.set('');
    this.selectedPatientDetail.set(null);
    this.selectedSessionDetail.set(null);
    localStorage.removeItem(TOKEN_KEY);
    localStorage.removeItem(USER_KEY);
    this.apiMessage.set('Sessao terminada localmente.');
    await this.router.navigate(['/login']);
  }

  updatePatient(field: keyof PatientForm, value: string) {
    this.patientForm.update((form) => ({ ...form, [field]: value }));
  }

  updatePatientEdit(field: keyof PatientEditForm, value: string) {
    this.patientEditForm.update((form) => ({ ...form, [field]: value }));
  }

  updateGoal(field: keyof GoalForm, value: string) {
    this.goalForm.update((form) => ({ ...form, [field]: value }));
  }

  updateSession(field: keyof SessionForm, value: string) {
    this.sessionForm.update((form) => {
      const next = { ...form, [field]: value };
      if (field === 'type') {
        const allowedGoalIds = new Set(this.goalIdsForSessionType(value));
        next.goalIds = form.goalIds.filter((goalId) => allowedGoalIds.has(goalId));
      }

      return next;
    });
  }

  updateCheckpoint(field: keyof CheckpointForm, value: string) {
    this.checkpointForm.update((form) => ({ ...form, [field]: value }));
  }

  async createPatient() {
    const form = this.patientForm();
    await this.runApi(async () => {
      const response = await firstValueFrom(this.http.post<{ id: string }>(`${API_BASE_URL}/patients`, {
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
      await this.goPatient(response.id);
      this.apiMessage.set('Paciente criado e aberto na ficha.');
    }, 'Nao foi possivel criar paciente.');
  }

  async savePatientEdit() {
    const form = this.patientEditForm();
    if (!form.id) {
      return;
    }

    await this.runApi(async () => {
      await firstValueFrom(this.http.put(`${API_BASE_URL}/patients`, {
        patient: {
          id: form.id,
          name: form.name,
          dateOfBirth: this.toIsoDate(form.dateOfBirth),
          patientAddress: {
            addressLine: form.addressLine,
            district: form.district,
            location: form.location,
            zipCode: form.zipCode,
          },
          mainDiagnosis: form.mainDiagnosis,
          gender: form.gender || null,
          phoneNumber: form.phoneNumber || null,
          email: form.email || null,
          caregiverName: form.caregiverName || null,
          caregiverPhone: form.caregiverPhone || null,
          referralReason: form.referralReason || null,
          generalNotes: form.generalNotes || null,
        },
      }, { headers: this.authHeaders() }));

      await this.loadWorkspace();
      await this.loadPatientContext(form.id);
      this.apiMessage.set('Paciente atualizado.');
    }, 'Nao foi possivel atualizar paciente.');
  }

  async deletePatient(patientId: string) {
    await this.runApi(async () => {
      await firstValueFrom(this.http.delete(`${API_BASE_URL}/patients/${patientId}`, { headers: this.authHeaders() }));
      await this.loadWorkspace();
      await this.goTherapist();
      this.apiMessage.set('Paciente arquivado.');
    }, 'Nao foi possivel arquivar paciente.');
  }

  async startEditingPatient(patient: Patient) {
    this.editingPatientId.set(patient.id);
    this.patientEditForm.set({
      id: patient.id,
      name: patient.name,
      dateOfBirth: patient.dateOfBirth.slice(0, 10),
      mainDiagnosis: patient.mainDiagnosis,
      gender: patient.gender ?? '',
      phoneNumber: patient.phoneNumber ?? '',
      email: patient.email ?? '',
      caregiverName: patient.caregiverName ?? '',
      caregiverPhone: patient.caregiverPhone ?? '',
      referralReason: patient.referralReason ?? '',
      generalNotes: patient.generalNotes ?? '',
      addressLine: patient.patientAddress?.addressLine ?? '',
      district: patient.patientAddress?.district ?? '',
      location: patient.patientAddress?.location ?? '',
      zipCode: patient.patientAddress?.zipCode ?? '',
    });
  }

  async createOrUpdateGoal() {
    const patientId = this.selectedPatientId();
    const form = this.goalForm();
    const editingGoalId = this.editingGoalId();
    if (!patientId) {
      return;
    }

    await this.runApi(async () => {
      if (editingGoalId) {
        await firstValueFrom(this.http.put(`${API_BASE_URL}/therapy-goals/${editingGoalId}`, {
          goal: {
            type: form.type,
            description: form.description,
            area: form.area,
            priority: form.priority,
            reviewDate: form.reviewDate ? this.toIsoDate(form.reviewDate) : null,
          },
        }, { headers: this.authHeaders() }));
      } else {
        await firstValueFrom(this.http.post(`${API_BASE_URL}/patients/${patientId}/therapy-goals`, {
          goal: {
            type: form.type,
            description: form.description,
            area: form.area,
            priority: form.priority,
            reviewDate: form.reviewDate ? this.toIsoDate(form.reviewDate) : null,
          },
        }, { headers: this.authHeaders() }));
      }

      await this.loadPatientContext(patientId, this.selectedSessionId());
      this.editingGoalId.set('');
      this.apiMessage.set(editingGoalId ? 'Objetivo atualizado.' : 'Objetivo criado.');
    }, 'Nao foi possivel guardar objetivo.');
  }

  async editGoal(goal: TherapeuticGoal) {
    this.editingGoalId.set(goal.id);
    this.goalForm.set({
      type: goal.type,
      description: goal.description,
      area: goal.area,
      priority: goal.priority,
      reviewDate: goal.reviewDate ? goal.reviewDate.slice(0, 10) : '',
    });
  }

  cancelGoalEditing() {
    this.editingGoalId.set('');
    this.goalForm.set({
      type: 'Objective',
      description: 'Melhorar coordenacao motora fina',
      area: 'Motricidade fina',
      priority: 'Medium',
      reviewDate: '2026-07-01',
    });
  }

  async updateGoalStatus(goalId: string, status: string) {
    const patientId = this.selectedPatientId();
    if (!patientId) {
      return;
    }

    await this.runApi(async () => {
      await firstValueFrom(this.http.patch(`${API_BASE_URL}/therapy-goals/${goalId}/status`, { status }, { headers: this.authHeaders() }));
      await this.loadPatientContext(patientId, this.selectedSessionId());
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
      const response = await firstValueFrom(this.http.post<{ id: string }>(`${API_BASE_URL}/patients/${patientId}/sessions`, {
        session: {
          startDateTime: this.toIsoDateTime(form.startDateTime),
          endDateTime: this.toIsoDateTime(form.endDateTime),
          type: form.type,
          location: form.location,
          goalIds: form.goalIds,
        },
      }, { headers: this.authHeaders() }));

      await this.loadWorkspace();
      await this.goSession(patientId, response.id);
      this.apiMessage.set('Sessao agendada.');
    }, 'Nao foi possivel agendar sessao.');
  }

  async saveSessionChanges() {
    const session = this.selectedSession();
    if (!session) {
      return;
    }

    const form = this.sessionForm();
    await this.runApi(async () => {
      await firstValueFrom(this.http.patch(`${API_BASE_URL}/sessions/${session.id}/reschedule`, {
        session: {
          startDateTime: this.toIsoDateTime(form.startDateTime),
          endDateTime: this.toIsoDateTime(form.endDateTime),
          type: form.type,
          location: form.location,
        },
      }, { headers: this.authHeaders() }));

      await firstValueFrom(this.http.patch(`${API_BASE_URL}/sessions/${session.id}/goals`, {
        goals: {
          goalIds: form.goalIds,
        },
      }, { headers: this.authHeaders() }));

      await this.loadPatientContext(session.patientId, session.id);
      this.apiMessage.set('Sessao atualizada.');
    }, 'Nao foi possivel atualizar a sessao.');
  }

  async cancelSession() {
    const session = this.selectedSession();
    if (!session) {
      return;
    }

    await this.runApi(async () => {
      await firstValueFrom(this.http.patch(`${API_BASE_URL}/sessions/${session.id}/cancel`, {
        session: { cancellationReason: 'Cancelada pelo terapeuta' },
      }, { headers: this.authHeaders() }));

      await this.loadPatientContext(session.patientId, session.id);
      this.apiMessage.set('Sessao cancelada.');
    }, 'Nao foi possivel cancelar a sessao.');
  }

  async completeSession(sessionId: string) {
    const patientId = this.selectedPatientId();
    const form = this.checkpointForm();
    if (!patientId) {
      return;
    }

    await this.runApi(async () => {
      await firstValueFrom(this.http.patch(`${API_BASE_URL}/sessions/${sessionId}/complete`, { session: form }, { headers: this.authHeaders() }));
      await this.loadPatientContext(patientId, sessionId);
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
      await this.loadPatientContext(patientId, this.selectedSessionId());
      this.apiMessage.set('Draft de relatorio gerado.');
    }, 'Nao foi possivel gerar o draft de relatorio.');
  }

  async exportReport(reportId: string, format: 'pdf' | 'word') {
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
      await this.loadPatientContext(patientId, this.selectedSessionId());
      this.apiMessage.set('Rascunho de relatorio descartado.');
    }, 'Nao foi possivel descartar o rascunho.');
  }

  async loadWorkspace() {
    await this.runApi(async () => {
      const [patientsResponse, sessionsResponse] = await Promise.all([
        firstValueFrom(this.http.get<{ patients: Patient[] }>(`${API_BASE_URL}/patients/me`, { headers: this.authHeaders() })),
        firstValueFrom(this.http.get<{ sessions: Session[] }>(`${API_BASE_URL}/sessions`, { headers: this.authHeaders() })),
      ]);

      this.patients.set(patientsResponse.patients);
      this.sessions.set(sessionsResponse.sessions);
      this.apiMessage.set('Workspace sincronizado com a API.');
    }, 'Nao consegui carregar o workspace. Confirma a API, Docker e a base de dados.');
  }

  async loadPatientContext(patientId: string, sessionId?: string | null) {
    if (!patientId || !this.token()) {
      return;
    }

    await this.runApi(async () => {
      const headers = this.authHeaders();
      const [patientResponse, sessionsResponse, goalsResponse, reportsResponse, dashboardResponse] = await Promise.all([
        firstValueFrom(this.http.get<{ patient: Patient }>(`${API_BASE_URL}/patients/id/${patientId}`, { headers })),
        firstValueFrom(this.http.get<{ sessions: Session[] }>(`${API_BASE_URL}/patients/${patientId}/sessions`, { headers })),
        firstValueFrom(this.http.get<{ therapeuticGoals: TherapeuticGoal[] }>(`${API_BASE_URL}/patients/${patientId}/therapy-goals`, { headers })),
        firstValueFrom(this.http.get<{ reports: Report[] }>(`${API_BASE_URL}/patients/${patientId}/reports`, { headers })),
        firstValueFrom(this.http.get<{ dashboard: ProgressDashboard }>(`${API_BASE_URL}/patients/${patientId}/progress-dashboard`, { headers })),
      ]);

      this.selectedPatientDetail.set(patientResponse.patient);
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

      const nextSessionId = sessionId || sessionsResponse.sessions[0]?.id || '';
      this.selectedSessionDetail.set(nextSessionId ? sessionsResponse.sessions.find((item) => item.id === nextSessionId) ?? null : null);

      if (this.selectedPatientDetail()) {
        this.patientEditForm.set({
          id: patientResponse.patient.id,
          name: patientResponse.patient.name,
          dateOfBirth: patientResponse.patient.dateOfBirth.slice(0, 10),
          mainDiagnosis: patientResponse.patient.mainDiagnosis,
          gender: patientResponse.patient.gender ?? '',
          phoneNumber: patientResponse.patient.phoneNumber ?? '',
          email: patientResponse.patient.email ?? '',
          caregiverName: patientResponse.patient.caregiverName ?? '',
          caregiverPhone: patientResponse.patient.caregiverPhone ?? '',
          referralReason: patientResponse.patient.referralReason ?? '',
          generalNotes: patientResponse.patient.generalNotes ?? '',
          addressLine: patientResponse.patient.patientAddress?.addressLine ?? '',
          district: patientResponse.patient.patientAddress?.district ?? '',
          location: patientResponse.patient.patientAddress?.location ?? '',
          zipCode: patientResponse.patient.patientAddress?.zipCode ?? '',
        });
      }

      if (sessionId) {
        const sessionResponse = await firstValueFrom(this.http.get<{ session: Session }>(`${API_BASE_URL}/sessions/${sessionId}`, { headers }));
        this.selectedSessionDetail.set(sessionResponse.session);
        this.sessionForm.set({
          startDateTime: this.toLocalDateTime(sessionResponse.session.startDateTime),
          endDateTime: this.toLocalDateTime(sessionResponse.session.endDateTime),
          type: sessionResponse.session.type,
          location: sessionResponse.session.location,
          goalIds: sessionResponse.session.goalIds ?? [],
        });
        this.checkpointForm.set({
          clinicalSummary: sessionResponse.session.clinicalSummary ?? '',
          objectivesWorked: sessionResponse.session.objectivesWorked ?? '',
          progressRating: sessionResponse.session.progressRating ?? '',
          activities: sessionResponse.session.activities ?? '',
          patientResponse: sessionResponse.session.patientResponse ?? '',
          difficulties: sessionResponse.session.difficulties ?? '',
          recommendations: sessionResponse.session.recommendations ?? '',
          nextSteps: sessionResponse.session.nextSteps ?? '',
        });
      } else {
        this.sessionForm.set({
          startDateTime: '2026-06-08T10:00',
          endDateTime: '2026-06-08T11:00',
          type: 'Intervention',
          location: 'Gabinete 1',
          goalIds: [],
        });
      }
    }, 'Nao foi possivel carregar o contexto do paciente.');
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

  goalTypeLabel(type: string) {
    return {
      Area: 'Area',
      Objective: 'Objetivo',
    }[type] ?? type;
  }

  sessionGoalContextLabel() {
    return this.sessionForm().type === 'Intervention'
      ? 'Objetivos'
      : 'Areas';
  }

  isSessionGoalSelected(goalId: string) {
    return this.sessionForm().goalIds.includes(goalId);
  }

  toggleSessionGoal(goalId: string) {
    this.sessionForm.update((form) => ({
      ...form,
      goalIds: form.goalIds.includes(goalId)
        ? form.goalIds.filter((item) => item !== goalId)
        : [...form.goalIds, goalId],
    }));
  }

  goalIdsForSessionType(sessionType: string) {
    return sessionType === 'Intervention'
      ? this.selectedPatientObjectives().map((goal) => goal.id)
      : this.selectedPatientAreas().map((goal) => goal.id);
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

  patientAddress(patient: Patient) {
    return [
      patient.patientAddress?.addressLine,
      patient.patientAddress?.zipCode,
      patient.patientAddress?.location,
      patient.patientAddress?.district,
    ].filter(Boolean).join(', ');
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

  private toLocalDateTime(value: string) {
    const date = new Date(value);
    const pad = (num: number) => `${num}`.padStart(2, '0');
    const year = date.getFullYear();
    const month = pad(date.getMonth() + 1);
    const day = pad(date.getDate());
    const hours = pad(date.getHours());
    const minutes = pad(date.getMinutes());
    return `${year}-${month}-${day}T${hours}:${minutes}`;
  }

  private extractFileName(disposition: string | null) {
    const match = disposition?.match(/filename="?([^"]+)"?/i);
    return match?.[1];
  }
}

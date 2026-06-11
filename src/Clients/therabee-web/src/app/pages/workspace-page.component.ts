import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, HostListener, computed, effect, inject, signal } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { firstValueFrom } from 'rxjs';
import { toSignal } from '@angular/core/rxjs-interop';

type WorkspacePage = 'therapist' | 'patient' | 'session';
type ActionModal = 'patient-create' | 'patient-edit' | 'patient-archive' | 'patient-delete' | 'preset' | 'session-create' | 'session-edit' | '';
type NotificationKind = 'info' | 'warning' | 'error';

type WorkspaceNotification = {
  message: string;
  kind: NotificationKind;
};

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
  priority: string;
  status: string;
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
  goalAssessments: SessionGoalAssessment[];
};

type SessionGoalAssessment = {
  id: string;
  sessionId: string;
  therapeuticGoalId: string;
  score: number;
  clinicalNotes?: string | null;
  sessionStartDateTime?: string | null;
  createdAt?: string | null;
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
  priority: string;
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

type GoalAssessmentFormEntry = {
  score: string;
  clinicalNotes: string;
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
  private notificationTimeout: ReturnType<typeof setTimeout> | null = null;

  readonly currentPage = computed<WorkspacePage>(() => (this.routeData()?.['page'] as WorkspacePage) ?? 'therapist');
  readonly token = signal(this.readStoredToken());
  readonly user = signal<AuthUser | null>(this.readStoredUser());
  readonly notification = signal<WorkspaceNotification | null>(null);
  readonly isBusy = signal(false);

  readonly patients = signal<Patient[]>([]);
  readonly sessions = signal<Session[]>([]);
  readonly goals = signal<TherapeuticGoal[]>([]);
  readonly reports = signal<Report[]>([]);
  readonly patientGoalAssessments = signal<SessionGoalAssessment[]>([]);
  readonly dashboard = signal<ProgressDashboard | null>(null);
  readonly selectedPatientId = signal('');
  readonly selectedSessionId = signal('');
  readonly selectedPatientDetail = signal<Patient | null>(null);
  readonly selectedSessionDetail = signal<Session | null>(null);
  readonly editingGoalId = signal('');
  readonly editingPatientId = signal('');
  readonly patientArchiveTarget = signal<Patient | null>(null);
  readonly patientDeleteTarget = signal<Patient | null>(null);
  readonly showArchivedPatients = signal(false);
  readonly activeModal = signal<ActionModal>('');

  readonly patientForm = signal<PatientForm>(this.createEmptyPatientForm());

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
    description: '',
    priority: 'Medium',
  });

  readonly sessionForm = signal<SessionForm>({
    startDateTime: '2026-06-08T10:00',
    endDateTime: '2026-06-08T11:00',
    type: 'Intervention',
    location: 'Gabinete 1',
    goalIds: [],
  });

  readonly checkpointForm = signal<CheckpointForm>({
    clinicalSummary: 'Sessão focada em regulação tónica e organização motora.',
    objectivesWorked: 'Coordenação bilateral; planeamento motor; atenção sustentada.',
    progressRating: 'Bom progresso',
    activities: 'Circuito motor, encaixes finos e sequencia de tarefas.',
    patientResponse: 'Boa adesão, com maior autonomia no final da sessão.',
    difficulties: 'Oscilação atencional em tarefas longas.',
    recommendations: 'Manter rotina de exercícios curtos em casa.',
    nextSteps: 'Rever objetivos e aumentar complexidade gradualmente.',
  });
  readonly assessmentForm = signal<Record<string, GoalAssessmentFormEntry>>({});

  readonly selectedPatient = computed(() => this.selectedPatientDetail() ?? this.patients().find((patient) => patient.id === this.selectedPatientId()) ?? null);
  readonly selectedSession = computed(() => this.selectedSessionDetail() ?? this.sessions().find((session) => session.id === this.selectedSessionId()) ?? null);
  readonly activePatientId = computed(() => this.selectedPatient()?.id ?? this.selectedPatientId());
  readonly selectedPatientGoals = computed(() => this.goals().filter((goal) => goal.patientId === this.activePatientId()));
  readonly selectedPatientAreas = computed(() => this.selectedPatientGoals().filter((goal) => goal.type === 'Area'));
  readonly selectedPatientObjectives = computed(() => this.selectedPatientGoals().filter((goal) => goal.type === 'Objective'));
  readonly availableSessionGoals = computed(() => this.sessionForm().type === 'Intervention'
    ? this.selectedPatientObjectives()
    : this.selectedPatientAreas());
  readonly selectedPatientSessions = computed(() => this.sessions().filter((session) => session.patientId === this.activePatientId()).sort((a, b) => new Date(a.startDateTime).getTime() - new Date(b.startDateTime).getTime()));
  readonly selectedPatientReports = computed(() => this.reports().filter((report) => report.patientId === this.activePatientId()).sort((a, b) => new Date(b.createdAt ?? 0).getTime() - new Date(a.createdAt ?? 0).getTime()));
  readonly selectedPatientGoalAssessments = computed(() => this.patientGoalAssessments().slice().sort((a, b) => new Date(b.createdAt ?? 0).getTime() - new Date(a.createdAt ?? 0).getTime()));
  readonly allSessions = computed(() => this.sessions().slice().sort((a, b) => new Date(a.startDateTime).getTime() - new Date(b.startDateTime).getTime()));
  readonly upcomingSessions = computed(() => this.allSessions().filter((session) => session.status === 'Scheduled' || session.status === 'Rescheduled'));
  readonly therapistName = computed(() => this.user()?.name || 'Terapeuta');
  readonly patientNameById = (patientId: string) => this.patients().find((patient) => patient.id === patientId)?.name ?? 'Paciente';
  readonly selectedSessionGoals = computed(() => {
    const patientGoals = new Map(this.selectedPatientGoals().map((goal) => [goal.id, goal]));
    const goalIds = this.sessionForm().goalIds ?? [];
    if (!goalIds.length) {
      return [];
    }

    return goalIds
      .map((goalId) => patientGoals.get(goalId))
      .filter((goal): goal is TherapeuticGoal => Boolean(goal));
  });
  readonly currentSessionLabel = computed(() => {
    const patient = this.selectedPatient();
    const session = this.selectedSession();
    if (!patient || !session) {
      return 'Sessão';
    }

    const sessionNumber = Math.max(this.selectedPatientSessions().findIndex((item) => item.id === session.id) + 1, 1);
    return `Sessão ${sessionNumber} | ${patient.name}, ${patient.calculatedAge} anos`;
  });

  readonly activePatientCount = computed(() => this.patients().filter((patient) => this.isActivePatient(patient)).length);
  readonly visiblePatients = computed(() => this.patients().filter((patient) =>
    this.showArchivedPatients() ? !this.isActivePatient(patient) : this.isActivePatient(patient)));
  readonly visiblePatientCount = computed(() => this.visiblePatients().length);
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
    this.patientGoalAssessments.set([]);
    this.dashboard.set(null);
    this.selectedPatientId.set('');
    this.selectedSessionId.set('');
    this.selectedPatientDetail.set(null);
    this.selectedSessionDetail.set(null);
    localStorage.removeItem(TOKEN_KEY);
    localStorage.removeItem(USER_KEY);
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
      if (field === 'startDateTime' && value) {
        next.endDateTime = this.addOneHour(value);
      }

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

  updateAssessment(goalId: string, field: keyof GoalAssessmentFormEntry, value: string) {
    this.assessmentForm.update((form) => ({
      ...form,
      [goalId]: {
        score: form[goalId]?.score ?? '0',
        clinicalNotes: form[goalId]?.clinicalNotes ?? '',
        [field]: value,
      },
    }));
  }

  openActionModal(modal: ActionModal) {
    if (modal === 'patient-create') {
      this.patientForm.set(this.createEmptyPatientForm());
    }

    this.activeModal.set(modal);
  }

  closeActionModal() {
    this.activeModal.set('');
  }

  private isActivePatient(patient: Patient) {
    return (patient.status ?? '').toLowerCase() === 'active';
  }

  private createEmptyPatientForm(): PatientForm {
    return {
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
    };
  }

  toggleArchivedPatients(checked: boolean) {
    this.showArchivedPatients.set(checked);
  }

  promptDeletePatient(patient: Patient) {
    this.patientDeleteTarget.set(patient);
    this.openActionModal('patient-delete');
  }

  promptArchivePatient(patient: Patient) {
    this.patientArchiveTarget.set(patient);
    this.openActionModal('patient-archive');
  }

  cancelPatientDeletion() {
    this.patientDeleteTarget.set(null);
    this.closeActionModal();
  }

  cancelPatientArchive() {
    this.patientArchiveTarget.set(null);
    this.closeActionModal();
  }

  @HostListener('document:keydown.escape')
  onEscapeKey() {
    if (this.activeModal()) {
      this.closeActionModal();
    }
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
      this.closeActionModal();
      this.showNotification('Paciente criado e aberto na ficha.');
    }, 'Não foi possível criar paciente.');
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
      this.closeActionModal();
      this.showNotification('Paciente atualizado.');
    }, 'Não foi possível atualizar paciente.');
  }

  async archivePatient(patientId: string) {
    await this.runApi(async () => {
      await firstValueFrom(this.http.patch(`${API_BASE_URL}/patients/${patientId}/status`, { status: 'Inactive' }, { headers: this.authHeaders() }));
      await this.loadWorkspace();
      await this.goTherapist();
      this.showNotification('Paciente arquivado.');
    }, 'Não foi possível arquivar paciente.');
  }

  async deletePatient(patientId: string) {
    await this.runApi(async () => {
      await firstValueFrom(this.http.delete(`${API_BASE_URL}/patients/${patientId}`, { headers: this.authHeaders() }));
      await this.loadWorkspace();
      await this.goTherapist();
      this.showNotification('Paciente eliminado.', 'warning');
    }, 'Não foi possível eliminar paciente.');
  }

  async confirmPatientArchive() {
    const target = this.patientArchiveTarget();
    if (!target) {
      return;
    }

    this.cancelPatientArchive();
    await this.archivePatient(target.id);
  }

  async confirmPatientDeletion() {
    const target = this.patientDeleteTarget();
    if (!target) {
      return;
    }

    this.cancelPatientDeletion();
    await this.deletePatient(target.id);
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
      let goalId = editingGoalId;

      if (editingGoalId) {
        await firstValueFrom(this.http.put(`${API_BASE_URL}/therapy-goals/${editingGoalId}`, {
          goal: {
            type: form.type,
            description: form.description,
            priority: form.priority,
          },
        }, { headers: this.authHeaders() }));
      } else {
        const response = await firstValueFrom(this.http.post<{ id: string }>(`${API_BASE_URL}/patients/${patientId}/therapy-goals`, {
          goal: {
            type: form.type,
            description: form.description,
            priority: form.priority,
          },
        }, { headers: this.authHeaders() }));

        goalId = response.id;
      }

      this.upsertGoalInState({
        id: goalId || editingGoalId || '',
        patientId,
        type: form.type as TherapeuticGoal['type'],
        description: form.description,
        priority: form.priority as TherapeuticGoal['priority'],
        status: editingGoalId
          ? this.goals().find((goal) => goal.id === editingGoalId)?.status ?? 'NotStarted'
          : 'NotStarted',
      });

      await this.refreshPatientGoals(patientId);
      await this.loadPatientContext(patientId, this.selectedSessionId());
      this.editingGoalId.set('');
      this.closeActionModal();
      this.showNotification(editingGoalId ? 'Objetivo atualizado.' : 'Objetivo criado.');
    }, 'Não foi possível guardar objetivo.');
  }

  async editGoal(goal: TherapeuticGoal) {
    this.editingGoalId.set(goal.id);
    this.goalForm.set({
      type: goal.type,
      description: goal.description,
      priority: goal.priority,
    });
    this.openActionModal('preset');
  }

  cancelGoalEditing() {
    this.editingGoalId.set('');
    this.goalForm.set({
      type: 'Objective',
      description: '',
      priority: 'Medium',
    });
  }

  async updateGoalStatus(goalId: string, status: string) {
    const patientId = this.selectedPatientId();
    if (!patientId) {
      return;
    }

    await this.runApi(async () => {
      await firstValueFrom(this.http.patch(`${API_BASE_URL}/therapy-goals/${goalId}/status`, { status }, { headers: this.authHeaders() }));
      await this.refreshPatientGoals(patientId);
      await this.loadPatientContext(patientId, this.selectedSessionId());
      this.showNotification('Estado do objetivo atualizado.');
    }, 'Não foi possível atualizar o estado do objetivo.');
  }

  async createSession() {
    const patientId = this.selectedPatientId();
    const form = this.sessionForm();
    if (!patientId) {
      return;
    }

    await this.runApi(async () => {
      const validationMessage = this.sessionDateValidationMessage(form);
      if (validationMessage) {
        this.showNotification(validationMessage, 'warning');
        return;
      }

      await firstValueFrom(this.http.post<{ id: string }>(`${API_BASE_URL}/patients/${patientId}/sessions`, {
        session: {
          startDateTime: this.toIsoDateTime(form.startDateTime),
          endDateTime: this.toIsoDateTime(form.endDateTime),
          type: form.type,
          location: form.location,
          goalIds: form.goalIds,
        },
      }, { headers: this.authHeaders() }));

      await this.loadWorkspace();
      await this.loadPatientContext(patientId, null);
      this.closeActionModal();
      this.showNotification('Sessão agendada.');
    }, 'Não foi possível agendar sessão.');
  }

  async saveSessionChanges() {
    const session = this.selectedSession();
    if (!session) {
      return;
    }

    const form = this.sessionForm();
    await this.runApi(async () => {
      const validationMessage = this.sessionDateValidationMessage(form);
      if (validationMessage) {
        this.showNotification(validationMessage, 'warning');
        return;
      }

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
      this.showNotification('Sessão atualizada.');
    }, 'Não foi possível atualizar a sessão.');
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
      this.showNotification('Sessão cancelada.', 'warning');
    }, 'Não foi possível cancelar a sessão.');
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
      this.showNotification('Avaliação registada e sessão concluída.');
    }, 'Não foi possível concluir a sessão.');
  }

  async confirmSession() {
    const session = this.selectedSession();
    if (!session) {
      return;
    }

    await this.runApi(async () => {
      await this.saveSessionAssessmentsInternal(session.id);
      await firstValueFrom(this.http.patch(`${API_BASE_URL}/sessions/${session.id}/complete`, { session: this.checkpointForm() }, { headers: this.authHeaders() }));

      await this.loadPatientContext(session.patientId, session.id);
      this.showNotification('Avaliações submetidas e sessão confirmada.');
    }, 'Não foi possível confirmar a sessão.');
  }

  async saveSessionAssessments() {
    const session = this.selectedSession();
    if (!session) {
      return;
    }

    await this.runApi(async () => {
      await this.saveSessionAssessmentsInternal(session.id);
      await this.loadPatientContext(session.patientId, session.id);
      this.showNotification('Avaliações guardadas.');
    }, 'Não foi possível guardar as avaliações.');
  }

  async createReportDraft() {
    const patientId = this.selectedPatientId();
    if (!patientId) {
      return;
    }

    await this.runApi(async () => {
      await firstValueFrom(this.http.post(`${API_BASE_URL}/patients/${patientId}/reports/draft`, {}, { headers: this.authHeaders() }));
      await this.loadPatientContext(patientId, this.selectedSessionId());
      this.showNotification('Rascunho de relatório gerado.');
    }, 'Não foi possível gerar o rascunho de relatório.');
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
      this.showNotification(`Relatório exportado em ${format.toUpperCase()}.`);
    }, `Não foi possível exportar em ${format.toUpperCase()}.`);
  }

  async discardReport(reportId: string) {
    const patientId = this.selectedPatientId();
    if (!patientId) {
      return;
    }

    await this.runApi(async () => {
      await firstValueFrom(this.http.delete(`${API_BASE_URL}/reports/${reportId}`, { headers: this.authHeaders() }));
      await this.loadPatientContext(patientId, this.selectedSessionId());
      this.showNotification('Rascunho de relatório descartado.', 'warning');
    }, 'Não foi possível descartar o rascunho.');
  }

  async loadWorkspace() {
    await this.runApi(async () => {
      const [patientsResponse, sessionsResponse] = await Promise.all([
        firstValueFrom(this.http.get<{ patients: Patient[] }>(`${API_BASE_URL}/patients/me`, { headers: this.authHeaders() })),
        firstValueFrom(this.http.get<{ sessions: Session[] }>(`${API_BASE_URL}/sessions`, { headers: this.authHeaders() })),
      ]);

      this.patients.set(patientsResponse.patients);
      this.sessions.set(sessionsResponse.sessions);
    }, 'Não consegui carregar o workspace. Confirma a API, o Docker e a base de dados.');
  }

  async loadPatientContext(patientId: string, sessionId?: string | null) {
    if (!patientId || !this.token()) {
      return;
    }

    await this.runApi(async () => {
      const headers = this.authHeaders();
      const patientResponse = await firstValueFrom(this.http.get<{ patient: Patient }>(`${API_BASE_URL}/patients/id/${patientId}`, { headers }));
      const [sessionsResponse, goalsResponse, reportsResponse, assessmentsResponse, dashboardResponse] = await Promise.allSettled([
        firstValueFrom(this.http.get<{ sessions: Session[] }>(`${API_BASE_URL}/patients/${patientId}/sessions`, { headers })),
        firstValueFrom(this.http.get<{ therapeuticGoals: TherapeuticGoal[] }>(`${API_BASE_URL}/patients/${patientId}/therapy-goals`, { headers })),
        firstValueFrom(this.http.get<{ reports: Report[] }>(`${API_BASE_URL}/patients/${patientId}/reports`, { headers })),
        firstValueFrom(this.http.get<{ assessments: SessionGoalAssessment[] }>(`${API_BASE_URL}/patients/${patientId}/goal-assessments`, { headers })),
        firstValueFrom(this.http.get<{ dashboard: ProgressDashboard }>(`${API_BASE_URL}/patients/${patientId}/progress-dashboard`, { headers })),
      ]);

      this.selectedPatientDetail.set(patientResponse.patient);
      this.sessions.update((sessions) => [
        ...sessions.filter((session) => session.patientId !== patientId),
        ...(sessionsResponse.status === 'fulfilled' ? sessionsResponse.value.sessions : []),
      ]);
      this.goals.update((goals) => [
        ...goals.filter((goal) => goal.patientId !== patientId),
        ...(goalsResponse.status === 'fulfilled' ? goalsResponse.value.therapeuticGoals : []),
      ]);
      this.reports.update((reports) => [
        ...reports.filter((report) => report.patientId !== patientId),
        ...(reportsResponse.status === 'fulfilled' ? reportsResponse.value.reports : []),
      ]);
      this.patientGoalAssessments.set(assessmentsResponse.status === 'fulfilled' ? assessmentsResponse.value.assessments : []);
      this.dashboard.set(dashboardResponse.status === 'fulfilled' ? dashboardResponse.value.dashboard : null);

      const sessionItems = sessionsResponse.status === 'fulfilled' ? sessionsResponse.value.sessions : [];
      const nextSessionId = sessionId || sessionItems[0]?.id || '';
      this.selectedSessionDetail.set(nextSessionId ? sessionItems.find((item) => item.id === nextSessionId) ?? null : null);

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
        const sessionResponse = await firstValueFrom(this.http.get<{ session: Session }>(`${API_BASE_URL}/sessions/${sessionId}`, { headers })).catch(() => null);
        if (sessionResponse) {
          this.selectedSessionDetail.set(sessionResponse.session);
        }
        this.sessionForm.set({
          startDateTime: sessionResponse ? this.toLocalDateTime(sessionResponse.session.startDateTime) : '2026-06-08T10:00',
          endDateTime: sessionResponse ? this.toLocalDateTime(sessionResponse.session.endDateTime) : '2026-06-08T11:00',
          type: sessionResponse ? sessionResponse.session.type : 'Intervention',
          location: sessionResponse ? sessionResponse.session.location : 'Gabinete 1',
          goalIds: sessionResponse ? sessionResponse.session.goalIds ?? [] : [],
        });
        this.assessmentForm.set(sessionResponse ? this.buildAssessmentForm(sessionResponse.session) : {});
        this.checkpointForm.set(sessionResponse ? {
          clinicalSummary: sessionResponse.session.clinicalSummary ?? '',
          objectivesWorked: sessionResponse.session.objectivesWorked ?? '',
          progressRating: sessionResponse.session.progressRating ?? '',
          activities: sessionResponse.session.activities ?? '',
          patientResponse: sessionResponse.session.patientResponse ?? '',
          difficulties: sessionResponse.session.difficulties ?? '',
          recommendations: sessionResponse.session.recommendations ?? '',
          nextSteps: sessionResponse.session.nextSteps ?? '',
        } : {
          clinicalSummary: '',
          objectivesWorked: '',
          progressRating: '',
          activities: '',
          patientResponse: '',
          difficulties: '',
          recommendations: '',
          nextSteps: '',
        });
      } else {
        this.sessionForm.set({
          startDateTime: '2026-06-08T10:00',
          endDateTime: '2026-06-08T11:00',
          type: 'Intervention',
          location: 'Gabinete 1',
          goalIds: [],
        });
        this.assessmentForm.set({});
      }
    }, 'Não consegui carregar a ficha completa do paciente. Confirma se o paciente ainda existe, se a sessão está válida e se a API está a responder.');
  }

  private async refreshPatientGoals(patientId: string) {
    const response = await firstValueFrom(this.http.get<{ therapeuticGoals: TherapeuticGoal[] }>(
      `${API_BASE_URL}/patients/${patientId}/therapy-goals`,
      { headers: this.authHeaders() },
    ));

    this.goals.update((goals) => this.mergeTherapeuticGoals(goals, response.therapeuticGoals, patientId));
  }

  private upsertGoalInState(goal: TherapeuticGoal) {
    if (!goal.id) {
      return;
    }

    this.goals.update((goals) => this.mergeTherapeuticGoals(goals, [goal], goal.patientId));
  }

  private mergeTherapeuticGoals(existingGoals: TherapeuticGoal[], refreshedGoals: TherapeuticGoal[], patientId: string) {
    const refreshedById = new Map(refreshedGoals.map((goal) => [goal.id, goal]));

    return [
      ...existingGoals.filter((goal) => goal.patientId !== patientId || refreshedById.has(goal.id)),
      ...refreshedGoals,
    ].filter((goal, index, goals) => goals.findIndex((item) => item.id === goal.id) === index);
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
      NotStarted: 'Não iniciado',
      InProgress: 'Em progresso',
      Achieved: 'Alcançado',
      Suspended: 'Suspenso',
    }[status] ?? status;
  }

  goalPriorityLabel(priority: string) {
    return {
      Low: 'Baixa',
      Medium: 'Média',
      High: 'Alta',
    }[priority] ?? priority;
  }

  goalTypeLabel(type: string) {
    return {
      Area: 'Área',
      Objective: 'Objetivo',
    }[type] ?? type;
  }

  goalDescriptionById(goalId: string) {
    return this.goals().find((goal) => goal.id === goalId)?.description ?? goalId;
  }

  sessionGoalContextLabel() {
    return this.sessionForm().type === 'Intervention'
      ? 'Objetivos'
      : 'Áreas';
  }

  isSessionGoalSelected(goalId: string) {
    return this.sessionForm().goalIds.includes(goalId);
  }

  toggleSessionGoal(goalId: string) {
    const shouldAdd = !this.sessionForm().goalIds.includes(goalId);

    this.sessionForm.update((form) => ({
      ...form,
      goalIds: shouldAdd
        ? [...form.goalIds, goalId]
        : form.goalIds.filter((item) => item !== goalId),
    }));

    this.assessmentForm.update((form) => {
      if (shouldAdd) {
        return {
          ...form,
          [goalId]: form[goalId] ?? { score: '0', clinicalNotes: '' },
        };
      }

      if (!form[goalId]) {
        return form;
      }

      const next = { ...form };
      delete next[goalId];
      return next;
    });
  }

  goalIdsForSessionType(sessionType: string) {
    return sessionType === 'Intervention'
      ? this.selectedPatientObjectives().map((goal) => goal.id)
      : this.selectedPatientAreas().map((goal) => goal.id);
  }

  private async saveSessionAssessmentsInternal(sessionId: string) {
    const goals = this.selectedSessionGoals();
    const form = this.assessmentForm();
    const assessments = goals.map((goal) => ({
      therapeuticGoalId: goal.id,
      score: Number(form[goal.id]?.score ?? 0),
      clinicalNotes: form[goal.id]?.clinicalNotes ?? '',
    }));

    await firstValueFrom(this.http.put(`${API_BASE_URL}/sessions/${sessionId}/goal-assessments`, {
      assessments: {
        assessments,
      },
    }, { headers: this.authHeaders() }));
  }

  sessionStatusLabel(status: string) {
    return {
      Scheduled: 'Agendada',
      Rescheduled: 'Reagendada',
      Completed: 'Concluída',
      Cancelled: 'Cancelada',
      NoShow: 'Falta',
    }[status] ?? status;
  }

  patientStatusLabel(status: string) {
    return {
      Active: 'Ativo',
      Inactive: 'Arquivado',
      Archived: 'Arquivado',
    }[status] ?? status;
  }

  sessionTypeLabel(type: string) {
    return {
      Assessment: 'Avaliação',
      Intervention: 'Terapêutica',
      Reassessment: 'Reavaliação',
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
      if (this.apiStatusCode(error) === 401) {
        await this.logout();
        return;
      }

      this.showNotification(this.apiErrorMessage(error, failureMessage), 'error');
    } finally {
      this.isBusy.set(false);
    }
  }

  private apiStatusCode(error: unknown) {
    return (error as { status?: number })?.status ?? 0;
  }

  private apiErrorMessage(error: unknown, fallback: string) {
    const candidate = error as { error?: unknown };
    const rawProblem = candidate?.error;

    if (typeof rawProblem === 'string' && rawProblem.trim()) {
      return rawProblem;
    }

    const problem = rawProblem as { detail?: string; title?: string; errors?: { message?: string }[] } | undefined;

    if (problem?.detail) {
      return problem.detail;
    }

    if (problem?.title) {
      return problem.title;
    }

    if (Array.isArray(problem?.errors) && problem.errors.length > 0) {
      const messages = problem.errors
        .map((item) => item.message)
        .filter(Boolean)
        .join(' ');

      if (messages) {
        return messages;
      }
    }

    return fallback;
  }

  dismissNotification() {
    if (this.notificationTimeout) {
      clearTimeout(this.notificationTimeout);
      this.notificationTimeout = null;
    }

    this.notification.set(null);
  }

  private showNotification(message: string, kind: NotificationKind = 'info') {
    if (this.notificationTimeout) {
      clearTimeout(this.notificationTimeout);
    }

    this.notification.set({ message, kind });
    this.notificationTimeout = setTimeout(() => {
      this.notification.set(null);
      this.notificationTimeout = null;
    }, kind === 'error' ? 8000 : 4500);
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

  private addOneHour(value: string) {
    const date = new Date(value);
    date.setHours(date.getHours() + 1);
    return this.toLocalDateTime(date.toISOString());
  }

  private sessionDateValidationMessage(form: SessionForm) {
    if (!form.startDateTime || !form.endDateTime) {
      return 'Define a data/hora de início e a data/hora de fim da sessão.';
    }

    const start = new Date(form.startDateTime);
    const end = new Date(form.endDateTime);

    if (Number.isNaN(start.getTime()) || Number.isNaN(end.getTime())) {
      return 'As datas da sessão não são válidas.';
    }

    if (end <= start) {
      return 'A data/hora de fim tem de ser posterior à data/hora de início.';
    }

    if (form.startDateTime.slice(0, 10) !== form.endDateTime.slice(0, 10)) {
      return 'A sessão tem de começar e terminar no mesmo dia. Sessões multi-dia não são suportadas.';
    }

    return '';
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

  private buildAssessmentForm(session: Session) {
    const form: Record<string, GoalAssessmentFormEntry> = {};

    for (const assessment of session.goalAssessments ?? []) {
      form[assessment.therapeuticGoalId] = {
        score: `${assessment.score}`,
        clinicalNotes: assessment.clinicalNotes ?? '',
      };
    }

    for (const goalId of session.goalIds ?? []) {
      if (!form[goalId]) {
        form[goalId] = {
          score: '0',
          clinicalNotes: '',
        };
      }
    }

    return form;
  }
}


import { Component, computed, signal } from '@angular/core';

type View = 'dashboard' | 'patients' | 'sessions' | 'notes';

type Patient = {
  id: string;
  name: string;
  age: number;
  therapist: string;
  diagnosis: string;
  address: string;
  nextSession: string;
  status: 'Active' | 'Review';
};

type Session = {
  id: string;
  patientId: string;
  date: string;
  type: string;
  status: 'Scheduled' | 'Draft notes';
};

type ClinicalNote = {
  patientId: string;
  text: string;
  createdAt: string;
};

@Component({
  selector: 'app-root',
  templateUrl: './app.html',
  styleUrl: './app.css',
})
export class App {
  readonly views: { id: View; label: string }[] = [
    { id: 'dashboard', label: 'Dashboard' },
    { id: 'patients', label: 'Patients' },
    { id: 'sessions', label: 'Sessions' },
    { id: 'notes', label: 'Clinical Notes' },
  ];

  readonly activeView = signal<View>('dashboard');
  readonly selectedPatientId = signal('p-001');
  readonly draftPatientId = signal('p-001');
  readonly draftNote = signal('');

  readonly patients = signal<Patient[]>([
    {
      id: 'p-001',
      name: 'Maria Silva',
      age: 34,
      therapist: 'Dr. Joao Ferreira',
      diagnosis: 'Psychomotor intervention follow-up',
      address: 'Rua das Flores 10, Lisboa',
      nextSession: '2026-06-08 10:00',
      status: 'Active',
    },
    {
      id: 'p-002',
      name: 'Rui Martins',
      age: 12,
      therapist: 'Dr. Joao Ferreira',
      diagnosis: 'Developmental coordination monitoring',
      address: 'Avenida Central 4, Setubal',
      nextSession: '2026-06-08 15:30',
      status: 'Active',
    },
    {
      id: 'p-003',
      name: 'Ana Costa',
      age: 41,
      therapist: 'Dr. Joao Ferreira',
      diagnosis: 'Occupational performance assessment',
      address: 'Rua Norte 7, Santarem',
      nextSession: '2026-06-10 09:30',
      status: 'Review',
    },
  ]);

  readonly sessions = signal<Session[]>([
    { id: 's-001', patientId: 'p-001', date: '2026-06-08 10:00', type: 'Psychomotor session', status: 'Scheduled' },
    { id: 's-002', patientId: 'p-002', date: '2026-06-08 15:30', type: 'Assessment', status: 'Scheduled' },
    { id: 's-003', patientId: 'p-003', date: '2026-06-10 09:30', type: 'Follow-up', status: 'Draft notes' },
  ]);

  readonly notes = signal<ClinicalNote[]>([
    { patientId: 'p-001', text: 'Improved attention span during structured activity.', createdAt: '2026-06-04' },
    { patientId: 'p-002', text: 'Needs additional motor planning exercises.', createdAt: '2026-06-03' },
  ]);

  readonly activeTitle = computed(() => this.views.find((view) => view.id === this.activeView())?.label ?? 'Dashboard');
  readonly selectedPatient = computed(() => this.patientById(this.selectedPatientId()) ?? this.patients()[0]);
  readonly patientCount = computed(() => this.patients().length);
  readonly sessionCount = computed(() => this.sessions().length);
  readonly noteCount = computed(() => this.notes().length);
  readonly upcomingSessions = computed(() => this.sessions().slice(0, 3));

  selectView(view: View) {
    this.activeView.set(view);
  }

  selectPatient(patientId: string) {
    this.selectedPatientId.set(patientId);
  }

  updateDraftPatient(patientId: string) {
    this.draftPatientId.set(patientId);
  }

  updateDraftNote(value: string) {
    this.draftNote.set(value);
  }

  addNote() {
    const text = this.draftNote().trim();

    if (!text) {
      return;
    }

    this.notes.update((notes) => [
      {
        patientId: this.draftPatientId(),
        text,
        createdAt: new Date().toISOString().slice(0, 10),
      },
      ...notes,
    ]);
    this.draftNote.set('');
  }

  patientById(patientId: string) {
    return this.patients().find((patient) => patient.id === patientId);
  }
}

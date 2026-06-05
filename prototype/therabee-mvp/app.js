const state = {
  selectedPatientId: "p-001",
  patients: [
    {
      id: "p-001",
      name: "Maria Silva",
      age: 34,
      therapist: "Dr. Joao Ferreira",
      diagnosis: "Psychomotor intervention follow-up",
      address: "Rua das Flores 10, Lisboa",
      nextSession: "2026-06-08 10:00",
      status: "Active"
    },
    {
      id: "p-002",
      name: "Rui Martins",
      age: 12,
      therapist: "Dr. Joao Ferreira",
      diagnosis: "Developmental coordination monitoring",
      address: "Avenida Central 4, Setubal",
      nextSession: "2026-06-08 15:30",
      status: "Active"
    },
    {
      id: "p-003",
      name: "Ana Costa",
      age: 41,
      therapist: "Dr. Joao Ferreira",
      diagnosis: "Occupational performance assessment",
      address: "Rua Norte 7, Santarem",
      nextSession: "2026-06-10 09:30",
      status: "Review"
    }
  ],
  sessions: [
    { id: "s-001", patientId: "p-001", date: "2026-06-08 10:00", type: "Psychomotor session", status: "Scheduled" },
    { id: "s-002", patientId: "p-002", date: "2026-06-08 15:30", type: "Assessment", status: "Scheduled" },
    { id: "s-003", patientId: "p-003", date: "2026-06-10 09:30", type: "Follow-up", status: "Draft notes" }
  ],
  notes: [
    { patientId: "p-001", text: "Improved attention span during structured activity.", createdAt: "2026-06-04" },
    { patientId: "p-002", text: "Needs additional motor planning exercises.", createdAt: "2026-06-03" }
  ]
};

const titles = {
  dashboard: "Dashboard",
  patients: "Patients",
  sessions: "Sessions",
  notes: "Clinical Notes"
};

const byId = (id) => document.getElementById(id);
const patientById = (id) => state.patients.find((patient) => patient.id === id);

function renderMetrics() {
  byId("metric-patients").textContent = state.patients.length;
  byId("metric-sessions").textContent = state.sessions.length;
  byId("metric-notes").textContent = state.notes.length;
}

function renderToday() {
  const todayList = byId("today-list");
  todayList.innerHTML = state.sessions
    .slice(0, 3)
    .map((session) => {
      const patient = patientById(session.patientId);
      return `
        <article class="row">
          <strong>${patient.name}</strong>
          <span class="meta">${session.date} · ${session.type}</span>
          <span class="tag">${session.status}</span>
        </article>
      `;
    })
    .join("");
}

function renderPatientList() {
  const list = byId("patient-list");
  list.innerHTML = state.patients
    .map((patient) => `
      <button class="patient-button ${patient.id === state.selectedPatientId ? "active" : ""}" data-patient-id="${patient.id}">
        <strong>${patient.name}</strong>
        <span class="meta">${patient.age} years · ${patient.diagnosis}</span>
        <span class="tag">${patient.status}</span>
      </button>
    `)
    .join("");

  list.querySelectorAll("[data-patient-id]").forEach((button) => {
    button.addEventListener("click", () => {
      state.selectedPatientId = button.dataset.patientId;
      renderPatientList();
      renderPatientDetail();
      renderFocusPatient();
    });
  });
}

function patientDetailsMarkup(patient) {
  return `
    <dl>
      <dt>Name</dt>
      <dd>${patient.name}</dd>
      <dt>Age</dt>
      <dd>${patient.age}</dd>
      <dt>Therapist</dt>
      <dd>${patient.therapist}</dd>
      <dt>Diagnosis</dt>
      <dd>${patient.diagnosis}</dd>
      <dt>Address</dt>
      <dd>${patient.address}</dd>
      <dt>Next session</dt>
      <dd>${patient.nextSession}</dd>
    </dl>
  `;
}

function renderPatientDetail() {
  byId("patient-detail").innerHTML = patientDetailsMarkup(patientById(state.selectedPatientId));
}

function renderFocusPatient() {
  byId("focus-patient").innerHTML = patientDetailsMarkup(patientById(state.selectedPatientId));
}

function renderSessions() {
  byId("session-list").innerHTML = state.sessions
    .map((session) => {
      const patient = patientById(session.patientId);
      return `
        <article class="row">
          <strong>${patient.name}</strong>
          <span class="meta">${session.date} · ${session.type}</span>
          <span class="tag">${session.status}</span>
        </article>
      `;
    })
    .join("");
}

function renderNoteForm() {
  byId("note-patient").innerHTML = state.patients
    .map((patient) => `<option value="${patient.id}">${patient.name}</option>`)
    .join("");
}

function renderNotes() {
  byId("notes-list").innerHTML = state.notes
    .map((note) => {
      const patient = patientById(note.patientId);
      return `
        <article class="note-item">
          <strong>${patient.name}</strong>
          <p>${note.text}</p>
          <span class="meta">${note.createdAt}</span>
        </article>
      `;
    })
    .join("");
}

function wireNavigation() {
  document.querySelectorAll("[data-view]").forEach((button) => {
    button.addEventListener("click", () => {
      const view = button.dataset.view;
      document.querySelectorAll(".nav-item").forEach((item) => item.classList.remove("active"));
      document.querySelectorAll(".view").forEach((section) => section.classList.remove("active"));
      button.classList.add("active");
      byId(view).classList.add("active");
      byId("view-title").textContent = titles[view];
    });
  });
}

function wireNotes() {
  byId("note-form").addEventListener("submit", (event) => {
    event.preventDefault();
    const text = byId("note-text").value.trim();
    if (!text) return;

    state.notes.unshift({
      patientId: byId("note-patient").value,
      text,
      createdAt: new Date().toISOString().slice(0, 10)
    });

    byId("note-text").value = "";
    renderMetrics();
    renderNotes();
  });
}

function render() {
  renderMetrics();
  renderToday();
  renderFocusPatient();
  renderPatientList();
  renderPatientDetail();
  renderSessions();
  renderNoteForm();
  renderNotes();
}

wireNavigation();
wireNotes();
render();


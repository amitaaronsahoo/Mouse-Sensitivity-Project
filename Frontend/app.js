// Base URL of the API
const API_URL = "http://localhost:5000/api/profiles";

//All Needed Constants
const form = document.getElementById("profile-form");
const idField = document.getElementById("profile-id");
const gameNameField = document.getElementById("game-name");
const fovField = document.getElementById("fov");
const dpiField = document.getElementById("dpi");
const sensitivityField = document.getElementById("sensitivity");
const cmPer360Field = document.getElementById("cm-per-360");
const notesField = document.getElementById("notes");

const submitBtn = document.getElementById("submit-btn");
const cancelBtn = document.getElementById("cancel-btn");
const tableBody = document.getElementById("profiles-body");
const emptyState = document.getElementById("empty-state");

// Reads the form thats gets sent to the server, returning an object with the values.
function readForm() {
  return {
    gameName: gameNameField.value,
    fieldOfView: Number(fovField.value),
    mouseDPI: Number(dpiField.value),
    inGameSensitivity: Number(sensitivityField.value),
    cmPer360: Number(cmPer360Field.value),
    notes: notesField.value || null,
  };
}

// Clears the form 
function resetForm() {
  form.reset();
  idField.value = "";
  submitBtn.textContent = "Save Profile";
  cancelBtn.classList.add("hidden");
}

// Gets the Forms
async function loadProfiles() {
  const response = await fetch(API_URL);
  const profiles = await response.json();
  renderProfiles(profiles);
}

// Builds a Table
function renderProfiles(profiles) {
  tableBody.innerHTML = "";
  emptyState.classList.toggle("hidden", profiles.length > 0);

  for (const profile of profiles) {
    const row = document.createElement("tr");
    row.innerHTML = `
      <td>${profile.gameName}</td>
      <td>${profile.fieldOfView}</td>
      <td>${profile.mouseDPI}</td>
      <td>${profile.inGameSensitivity}</td>
      <td>${profile.cmPer360}</td>
      <td>${profile.notes ?? ""}</td>
      <td class="actions-cell">
        <button class="edit-btn" data-id="${profile.id}">Edit</button>
        <button class="delete-btn" data-id="${profile.id}">Delete</button>
      </td>
    `;
    tableBody.appendChild(row);
  }

  // buttons for the table
  //each button gets a click handler 
  tableBody.querySelectorAll(".edit-btn").forEach((btn) => {
    btn.addEventListener("click", () => startEdit(profiles, Number(btn.dataset.id)));
  });
  tableBody.querySelectorAll(".delete-btn").forEach((btn) => {
    btn.addEventListener("click", () => deleteProfile(Number(btn.dataset.id)));
  });
}

// Populates the form with an existing profile's values so it can be edited.
function startEdit(profiles, id) {
  const profile = profiles.find((p) => p.id === id);
  if (!profile) return;

  idField.value = profile.id;
  gameNameField.value = profile.gameName;
  fovField.value = profile.fieldOfView;
  dpiField.value = profile.mouseDPI;
  sensitivityField.value = profile.inGameSensitivity;
  cmPer360Field.value = profile.cmPer360;
  notesField.value = profile.notes ?? "";

  submitBtn.textContent = "Update Profile";
  cancelBtn.classList.remove("hidden");
  window.scrollTo({ top: 0, behavior: "smooth" });
}

// POST /api/profiles - create a profile, or PUT /api/profiles/{id} - update
// an existing one, depending on whether we're in "edit" mode.
async function saveProfile(event) {
  event.preventDefault();

  const body = readForm();
  const editingId = idField.value;

  const response = editingId
    ? await fetch(`${API_URL}/${editingId}`, {
        method: "PUT",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(body),
      })
    : await fetch(API_URL, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(body),
      });

  if (!response.ok) {
    alert("Failed to save profile.");
    return;
  }

  resetForm();
  await loadProfiles();
}

// DELETE /api/profiles/{id} - remove a profile after user confirmation.
async function deleteProfile(id) {
  if (!confirm("Delete this profile?")) return;

  const response = await fetch(`${API_URL}/${id}`, { method: "DELETE" });
  if (!response.ok) {
    alert("Failed to delete profile.");
    return;
  }

  await loadProfiles();
}

form.addEventListener("submit", saveProfile);
cancelBtn.addEventListener("click", resetForm);

// Load the current profiles as soon as the page opens.
loadProfiles();

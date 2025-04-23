document.addEventListener("DOMContentLoaded", cargarForms);

const API_URL = 'https://localhost:7287/api/Form';

document.getElementById("formForm").addEventListener("submit", async function (e) {
  e.preventDefault();

  // Validación de campos vacíos
  const requiredFields = ["name", "description", "url"];
  let allFilled = true;

  requiredFields.forEach(field => {
    const value = document.getElementById(field).value.trim();
    if (!value) {
      allFilled = false;
      document.getElementById(field).classList.add("is-invalid");
    } else {
      document.getElementById(field).classList.remove("is-invalid");
    }
  });

  if (!allFilled) {
    alert("Por favor, complete todos los campos obligatorios.");
    return;
  }

  const id = parseInt(document.getElementById("formId").value) || 0;

  const form = {
    id,
    name: document.getElementById("name").value,
    description: document.getElementById("description").value,
    url: document.getElementById("url").value,
  };

  try {
    const method = id > 0 ? 'PUT' : 'POST';

    await fetch(API_URL, {
      method,
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(form)
    });

    alert(id > 0 ? "Form actualizado correctamente" : "Form registrado correctamente");
    this.reset();
    document.getElementById("formId").value = "";
    cargarForms();
  } catch (err) {
    console.error("Error al guardar form:", err);
    alert("Error al guardar form");
  }
});

async function cargarForms() {
  try {
    const response = await fetch(API_URL);
    const data = await response.json();

    const tbody = document.querySelector("#formsTable tbody");
    tbody.innerHTML = "";

    data.forEach(f => {
      const fila = `<tr>
        <td>${f.name || ''}</td>
        <td>${f.description || ''}</td>
        <td>${f.url || ''}</td>
        <td>
          <button onclick="editarForm(${f.id})" class="btn btn-warning btn-sm">Editar</button>
          <button onclick="eliminarLogico(${f.id})" class="btn btn-secondary btn-sm">Eliminar Lógico</button>
          <button onclick="eliminarDefinitivo(${f.id})" class="btn btn-danger btn-sm">Eliminar Permanente</button>
        </td>
      </tr>`;
      tbody.innerHTML += fila;
    });
  } catch (error) {
    console.error("Error al cargar forms:", error);
    alert("Error al cargar formularios");
  }
}

window.editarForm = async (id) => {
  try {
    const response = await fetch(`${API_URL}/${id}`);
    const f = await response.json();
    document.getElementById("formId").value = f.id;
    document.getElementById("name").value = f.name || '';
    document.getElementById("description").value = f.description || '';
    document.getElementById("url").value = f.url || '';
  } catch (error) {
    console.error("Error al editar:", error);
    alert("Error al cargar datos para editar");
  }
};

// Eliminar lógico
window.eliminarLogico = async (id) => {
  if (confirm("¿Eliminar lógicamente este formulario?")) {
    try {
      await fetch(`${API_URL}/Logico/${id}`, { method: 'PUT' });
      alert("Formulario eliminado lógicamente");
      cargarForms();
    } catch (error) {
      console.error("Error al eliminar lógicamente:", error);
      alert("Error al eliminar lógicamente");
    }
  }
};

window.eliminarDefinitivo = async (id) => {
  if (confirm("¿ELIMINAR PERMANENTEMENTE este formulario? Esta acción no se puede deshacer.")) {
    try {
      await fetch(`${API_URL}/permanent/${id}`, { method: 'DELETE' });
      alert("Formulario eliminado permanentemente");
      cargarForms();
    } catch (error) {
      console.error("Error al eliminar permanentemente:", error);
      alert("Error al eliminar permanentemente");
    }
  }
};

document.addEventListener("DOMContentLoaded", cargarFormModule);

const API_URL = 'https://localhost:7287/api/FormModule';

document.getElementById("formModule").addEventListener("submit", async function (e) {
  e.preventDefault();

  const id = parseInt(document.getElementById("formModuleId").value) || 0;
  const formId = document.getElementById("formId").value.trim();
  const moduleId = document.getElementById("moduleId").value.trim();

  // Validación de campos vacíos
  if (!formId || !moduleId) {
    alert("Por favor, complete todos los campos obligatorios.");
    return;
  }

  const formModule = {
    id,
    formId,
    moduleId
  };

  try {
    const method = id > 0 ? 'PUT' : 'POST';
    await fetch(API_URL, {
      method,
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(formModule)
    });

    if (!response.ok) {
      throw new Error(`Error en la solicitud: ${response.status} ${response.statusText}`);
    }

    alert(id > 0 ? "formModule actualizado correctamente" : "formModule registrado correctamente");
    this.reset();
    document.getElementById("formModuleId").value = "";
    cargarFormModule();
  } catch (err) {
    console.error("Error al guardar formModule:", err);
    alert("Error al guardar formModule");
  }
});

async function cargarFormModule() {
  try {
    const response = await fetch(API_URL);
    if (!response.ok) {
      throw new Error(`Error al cargar formModules: ${response.status} ${response.statusText}`);
    }
    const data = await response.json();

    const tbody = document.querySelector("#rolesTable tbody");
    tbody.innerHTML = "";

    data.forEach(fm => {
      const fila = `<tr>
        <td>${fm.formId || ''}</td>
        <td>${fm.moduleId || ''}</td>
        <td>
          <button onclick="editarRol(${fm.id})" class="btn btn-warning btn-sm">Editar</button>
          <button onclick="eliminarLogico(${fm.id})" class="btn btn-secondary btn-sm">Eliminar Lógico</button>
          <button onclick="eliminarDefinitivo(${fm.id})" class="btn btn-danger btn-sm">Eliminar Permanente</button>
        </td>
      </tr>`;
      tbody.innerHTML += fila;
    });
  } catch (error) {
    console.error("Error al cargar formModules:", error);
    alert("Error al cargar formModules");
  }
}

window.editarRol = async (id) => {
  try {
    const response = await fetch(`${API_URL}/${id}`);
    if (!response.ok) {
      throw new Error(`Error al obtener formModule: ${response.status} ${response.statusText}`);
    }
    const fm = await response.json();

    document.getElementById("formModuleId").value = fm.id;
    document.getElementById("formId").value = fm.formId || '';
    document.getElementById("moduleId").value = fm.moduleId || '';
  } catch (error) {
    console.error("Error al editar:", error);
    alert("Error al cargar datos para editar");
  }
};

window.eliminarLogico = async (id) => {
  if (confirm("¿Eliminar lógicamente este formModule?")) {
    try {
      const response = await fetch(`${API_URL}/Logico/${id}`, { method: 'PUT' });
      if (!response.ok) {
        throw new Error(`Error al eliminar lógicamente: ${response.status} ${response.statusText}`);
      }
      alert("formModule eliminado lógicamente");
      cargarFormModule();
    } catch (error) {
      console.error("Error al eliminar lógicamente:", error);
      alert("Error al eliminar lógicamente");
    }
  }
};

window.eliminarDefinitivo = async (id) => {
  if (confirm("¿ELIMINAR PERMANENTEMENTE este formModule? Esta acción no se puede deshacer.")) {
    try {
      const response = await fetch(`${API_URL}/permanent/${id}`, { method: 'DELETE' });
      if (!response.ok) {
        throw new Error(`Error al eliminar permanentemente: ${response.status} ${response.statusText}`);
      }
      alert("formModule eliminado permanentemente");
      cargarFormModule();
    } catch (error) {
      console.error("Error al eliminar permanentemente:", error);
      alert("Error al eliminar permanentemente");
    }
  }
};

document.addEventListener("DOMContentLoaded", cargarModules);

const API_URL = 'https://localhost:7287/api/Module';

document.getElementById("moduleForm").addEventListener("submit", async function (e) {
  e.preventDefault();

  // Validación de campos vacíos
  const requiredFields = ["name", "description"];
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
  
  const id = parseInt(document.getElementById("moduleId").value) || 0;
  const module = {
    id,
    name: document.getElementById("name").value,
    description: document.getElementById("description").value,
  };

  try {
    const method = id > 0 ? 'PUT' : 'POST';

    await fetch(API_URL, {
      method,
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(module)
    });

    alert(id > 0 ? "Módulo actualizado correctamente" : "Módulo registrado correctamente");
    this.reset();
    document.getElementById("moduleId").value = "";
    cargarModules();
  } catch (err) {
    console.error("Error al guardar módulo:", err);
    alert("Error al guardar módulo");
  }
});

async function cargarModules() {
  try {
    const response = await fetch(API_URL);
    const data = await response.json();

    const tbody = document.querySelector("#formsTable tbody");
    tbody.innerHTML = "";

    data.forEach(m => {
      const fila = `<tr>
        <td>${m.name || ''}</td>
        <td>${m.description || ''}</td>
        <td>
          <button onclick="editarModule(${m.id})" class="btn btn-warning btn-sm">Editar</button>
          <button onclick="eliminarLogico(${m.id})" class="btn btn-secondary btn-sm">Eliminar Lógico</button>
          <button onclick="eliminarDefinitivo(${m.id})" class="btn btn-danger btn-sm">Eliminar Permanente</button>
        </td>
      </tr>`;
      tbody.innerHTML += fila;
    });
  } catch (error) {
    console.error("Error al cargar módulos:", error);
    alert("Error al cargar módulos");
  }
}

window.editarModule = async (id) => {
  try {
    const response = await fetch(`${API_URL}/${id}`);
    const m = await response.json();

    document.getElementById("moduleId").value = m.id;
    document.getElementById("name").value = m.name || '';
    document.getElementById("description").value = m.description || '';
  } catch (error) {
    console.error("Error al editar módulo:", error);
    alert("Error al cargar datos para editar");
  }
};

window.eliminarLogico = async (id) => {
  if (confirm("¿Eliminar lógicamente este módulo?")) {
    try {
      await fetch(`${API_URL}/Logico/${id}`, { method: 'PUT' });
      alert("Módulo eliminado lógicamente");
      cargarModules();
    } catch (error) {
      console.error("Error al eliminar lógicamente:", error);
      alert("Error al eliminar lógicamente");
    }
  }
};

window.eliminarDefinitivo = async (id) => {
  if (confirm("¿ELIMINAR PERMANENTEMENTE este módulo? Esta acción no se puede deshacer.")) {
    try {
      await fetch(`${API_URL}/permanent/${id}`, { method: 'DELETE' });
      alert("Módulo eliminado permanentemente");
      cargarModules();
    } catch (error) {
      console.error("Error al eliminar permanentemente:", error);
      alert("Error al eliminar permanentemente");
    }
  }
};

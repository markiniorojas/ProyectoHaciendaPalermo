document.addEventListener("DOMContentLoaded", cargarPermissions);

const API_URL = 'https://localhost:7287/api/Permission';

document.getElementById("permissionForm").addEventListener("submit", async function (e) {
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

  const id = parseInt(document.getElementById("permissionId").value) || 0;
  const permission = {
    id,
    name: document.getElementById("name").value,
    description: document.getElementById("description").value,
  };

  try {
    const method = id > 0 ? 'PUT' : 'POST';

    await fetch(API_URL, {
      method,
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(permission)
    });

    alert(id > 0 ? "Permiso actualizado correctamente" : " Permiso registrado correctamente");
    this.reset();
    document.getElementById("permissionId").value = "";
    cargarPermissions();
  } catch (err) {
    console.error("Error al guardar permiso:", err);
    alert(" Error al guardar permiso");
  }
});

async function cargarPermissions() {
  try {
    const response = await fetch(API_URL);
    const data = await response.json();

    const tbody = document.querySelector("#permissionsTable tbody");
    tbody.innerHTML = "";

    data.forEach(p => {
      const fila = `<tr>
        <td>${p.name || ''}</td>
        <td>${p.description || ''}</td>
        <td>
          <button onclick="editarPermission(${p.id})" class="btn btn-warning btn-sm">Editar</button>
          <button onclick="eliminarLogico(${p.id})" class="btn btn-secondary btn-sm">Eliminar Lógico</button>
          <button onclick="eliminarDefinitivo(${p.id})" class="btn btn-danger btn-sm">Eliminar Permanente</button>
        </td>
      </tr>`;
      tbody.innerHTML += fila;
    });
  } catch (error) {
    console.error("Error al cargar permisos:", error);
    alert(" Error al cargar permisos");
  }
}

window.editarPermission = async (id) => {
  try {
    const response = await fetch(`${API_URL}/${id}`);
    const p = await response.json();

    document.getElementById("permissionId").value = p.id;
    document.getElementById("name").value = p.name || '';
    document.getElementById("description").value = p.description || '';
  } catch (error) {
    console.error("Error al editar permiso:", error);
    alert("Error al cargar datos para editar");
  }
};

window.eliminarLogico = async (id) => {
  if (confirm("¿Eliminar lógicamente este permiso?")) {
    try {
      await fetch(`${API_URL}/Logico/${id}`, { method: 'PUT' });
      alert("Permiso eliminado lógicamente");
      cargarPermissions();
    } catch (error) {
      console.error("Error al eliminar lógicamente:", error);
      alert("Error al eliminar lógicamente");
    }
  }
};

window.eliminarDefinitivo = async (id) => {
  if (confirm("¿ELIMINAR PERMANENTEMENTE este permiso? Esta acción no se puede deshacer.")) {
    try {
      await fetch(`${API_URL}/permanent/${id}`, { method: 'DELETE' });
      alert("Permiso eliminado permanentemente");
      cargarPermissions();
    } catch (error) {
      console.error("Error al eliminar permanentemente:", error);
      alert("Error al eliminar permanentemente");
    }
  }
};

document.addEventListener("DOMContentLoaded", cargarRoles);

const API_URL = 'https://localhost:7287/api/Rol';

document.getElementById("rolForm").addEventListener("submit", async function (e) {
  e.preventDefault();

  const id = parseInt(document.getElementById("rolId").value) || 0;

  const rol = {
    id,
    name: document.getElementById("name").value,
    description: document.getElementById("description").value
  };

  try {
    const method = id > 0 ? 'PUT' : 'POST';
    await fetch(API_URL, {
      method,
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(rol)
    });

    alert(id > 0 ? "Rol actualizado correctamente" : "Rol registrado correctamente");
    this.reset();
    document.getElementById("rolId").value = "";
    cargarRoles();
  } catch (err) {
    console.error("Error al guardar rol:", err);
    alert("Error al guardar rol");
  }
});

async function cargarRoles() {
  try {
    const response = await fetch(API_URL);
    const data = await response.json();

    const tbody = document.querySelector("#rolesTable tbody");
    tbody.innerHTML = "";

    data.forEach(r => {
      const fila = `<tr>
        <td>${r.id}</td>
        <td>${r.name || ''}</td>
        <td>${r.description || ''}</td>
        <td>
          <button onclick="editarRol(${r.id})" class="btn btn-warning btn-sm">Editar</button>
          <button onclick="eliminarLogico(${r.id})" class="btn btn-secondary btn-sm">Eliminar Lógico</button>
          <button onclick="eliminarDefinitivo(${r.id})" class="btn btn-danger btn-sm">Eliminar Permanente</button>
        </td>
      </tr>`;
      tbody.innerHTML += fila;
    });
  } catch (error) {
    console.error("Error al cargar roles:", error);
    alert("Error al cargar roles");
  }
}

window.editarRol = async (id) => {
  try {
    const response = await fetch(`${API_URL}/${id}`);
    const r = await response.json();

    document.getElementById("rolId").value = r.id;
    document.getElementById("name").value = r.name || '';
    document.getElementById("description").value = r.description || '';
  } catch (error) {
    console.error("Error al editar:", error);
    alert("Error al cargar datos para editar");
  }
};

window.eliminarLogico = async (id) => {
  if (confirm("¿Eliminar lógicamente este rol?")) {
    try {
      await fetch(`${API_URL}/Logico/${id}`, { method: 'PUT' });
      alert("Rol eliminado lógicamente");
      cargarRoles();
    } catch (error) {
      console.error("Error al eliminar lógicamente:", error);
      alert("Error al eliminar lógicamente");
    }
  }
};

window.eliminarDefinitivo = async (id) => {
  if (confirm(" ¿ELIMINAR PERMANENTEMENTE este rol? Esta acción no se puede deshacer.")) {
    try {
      await fetch(`${API_URL}/permanent/${id}`, { method: 'DELETE' });
      alert("Rol eliminado permanentemente");
      cargarRoles();
    } catch (error) {
      console.error("Error al eliminar permanentemente:", error);
      alert("Error al eliminar permanentemente");
    }
  }
};

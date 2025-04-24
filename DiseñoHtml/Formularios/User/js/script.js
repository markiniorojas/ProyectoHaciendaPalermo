// Script para gestionar CRUD de Usuarios
document.addEventListener("DOMContentLoaded", initialize);

const API_URL = 'https://localhost:7287/api/User';

function initialize() {
  cargarUsuarios();
  
  document.getElementById("userForm").addEventListener("submit", handleSubmit);
}

async function handleSubmit(e) {
  e.preventDefault();

  const requiredFields = ["email", "personId", "registrationDate"];
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
    showMessage("Por favor, complete todos los campos obligatorios.", "danger");
    return;
  }

  const id = parseInt(document.getElementById("userId").value) || 0;
  const isNewUser = id === 0;

  const password = document.getElementById("password").value;
  
  const user = {
    id,
    email: document.getElementById("email").value,
    isDeleted: document.getElementById("isDeleted").checked,
    personId: parseInt(document.getElementById("personId").value),
    registrationDate: document.getElementById("registrationDate").value
  };

  if (password) {
    user.password = password;
  }

  try {
    const method = isNewUser ? 'POST' : 'PUT';
    
    const response = await fetch(API_URL, {
      method,
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(user)
    });
    
    if (!response.ok) {
      const error = await response.json();
      throw new Error(error.message || "Error al procesar la solicitud");
    }

    showMessage(
      isNewUser ? "Usuario registrado correctamente" : "Usuario actualizado correctamente", 
      "success"
    );
    
    resetForm();
    cargarUsuarios();
  } catch (err) {
    console.error("Error:", err);
    showMessage(`Error: ${err.message}`, "danger");
  }
}

async function cargarUsuarios() {
  try {
    const response = await fetch(API_URL);
    
    if (!response.ok) {
      throw new Error("Error al obtener usuarios");
    }
    
    const data = await response.json();
    
    const tbody = document.querySelector("#userTable tbody");
    tbody.innerHTML = "";
    
    if (data.length === 0) {
      tbody.innerHTML = `<tr><td colspan="5" class="text-center">No hay usuarios registrados</td></tr>`;
      return;
    }
    
    data.forEach(u => {
      const estadoClass = u.isDeleted ? 'text-success' : 'text-danger';
      const estadoText = u.isDeleted ? 'Activo' : 'Inactivo';
      
      const fila = `<tr>
        <td>${u.email || ''}</td>
        <td><span class="badge bg-secondary">Oculta</span></td>
        <td><span class="${estadoClass}">${estadoText}</span></td>
        <td>${u.personId || ''}</td>
        <td>
          <button onclick="editarUsuario(${u.id})" class="btn btn-warning btn-sm text-white">
            <i class="bi bi-pencil"></i> Editar
          </button>
          <button onclick="eliminarLogico(${u.id})" class="btn btn-secondary btn-sm">
            <i class="bi bi-archive"></i> Desactivar
          </button>
          <button onclick="eliminarDefinitivo(${u.id})" class="btn btn-danger btn-sm">
            <i class="bi bi-trash"></i> Eliminar
          </button>
        </td>
      </tr>`;

      tbody.innerHTML += fila;
    });
  } catch (error) {
    console.error("Error al cargar usuarios:", error);
    showMessage("Error al cargar usuarios", "danger");
  }
}

// Editar usuario
window.editarUsuario = async (id) => {
  try {
    const response = await fetch(`${API_URL}/${id}`);
    
    if (!response.ok) {
      throw new Error("Error al obtener usuario");
    }
    
    const u = await response.json();
    
    document.getElementById("userId").value = u.id;
    document.getElementById("email").value = u.email || '';
    document.getElementById("password").value = ''; // No mostrar contraseña actual
    document.getElementById("isDeleted").checked = u.isDeleted || false;
    document.getElementById("personId").value = u.personId || '';
    document.getElementById("registrationDate").value = formatDate(u.registrationDate);
    
    // Desplazar a la parte superior del formulario
    document.querySelector(".container1").scrollIntoView({ behavior: 'smooth' });
  } catch (error) {
    console.error("Error al editar:", error);
    showMessage("Error al cargar datos para editar", "danger");
  }
}; 

// Eliminar lógico (desactivar)
window.eliminarLogico = async (id) => {
  if (confirm("¿Desea desactivar este usuario?")) {
    try {
      const response = await fetch(`${API_URL}/Logico/${id}`, { method: 'PUT' });
      
      if (!response.ok) {
        throw new Error("Error al desactivar usuario");
      }
      
      showMessage("Usuario desactivado correctamente", "success");
      cargarUsuarios();
    } catch (error) {
      console.error("Error al eliminar lógicamente:", error);
      showMessage("Error al desactivar usuario", "danger");
    }
  }
};

// Reactivar el objeto lógico (activar)
window.activadorLogico = async (id) => {
  if (confirm("¿Desea volver a activar este usuario?")) {
    try {
      const response = await fetch(`${API_URL}/recuperarLogica/{id:int}`, { method: 'PATCH' });
      
      if (!response.ok) {
        throw new Error("Error al activar usuario");
      }
      
      showMessage("Usuario activado correctamente", "success");
      cargarUsuarios();
    } catch (error) {
      console.error("Error al activar lógicamente:", error);
      showMessage("Error al activar usuario", "danger");
    }
  }
};

// Eliminar permanente
window.eliminarDefinitivo = async (id) => {
  if (confirm("¿ELIMINAR PERMANENTEMENTE este usuario? Esta acción no se puede deshacer.")) {
    try {
      const response = await fetch(`${API_URL}/permanent/${id}`, {method: 'DELETE'});
      
      if (!response.ok) {
        throw new Error("Error al eliminar usuario");
      }
      
      showMessage("Usuario eliminado permanentemente", "success");
      cargarUsuarios();
    } catch (error) {
      console.error("Error al eliminar permanentemente:", error);
      showMessage("Error al eliminar permanentemente", "danger");
    }
  }
};

// Utilidades
function resetForm() {
  document.getElementById("userForm").reset();
  document.getElementById("userId").value = "";
  
  // Quitar clase de error de todos los campos
  const formElements = document.getElementById("userForm").elements;
  for (let i = 0; i < formElements.length; i++) {
    formElements[i].classList.remove("is-invalid");
  }
}

function formatDate(dateString) {
  if (!dateString) return '';
  
  // Convertir la fecha a formato YYYY-MM-DD para el input date
  const date = new Date(dateString);
  return date.toISOString().split('T')[0];
}

// Mostrar mensajes de alerta
function showMessage(message, type = 'info') {
  // Eliminar alertas anteriores
  const existingAlerts = document.querySelectorAll('.alert');
  existingAlerts.forEach(alert => alert.remove());
  
  // Crear nueva alerta
  const alertDiv = document.createElement('div');
  alertDiv.className = `alert alert-${type} alert-dismissible fade show`;
  alertDiv.role = 'alert';
  alertDiv.innerHTML = `
    ${message}
    <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
  `;
  
  // Insertar al inicio del contenedor principal
  const container = document.querySelector('.container');
  container.insertBefore(alertDiv, container.firstChild);
  
  // Auto-cerrar después de 5 segundos
  setTimeout(() => {
    alertDiv.classList.remove('show');
    setTimeout(() => alertDiv.remove(), 300);
  }, 5000);
}
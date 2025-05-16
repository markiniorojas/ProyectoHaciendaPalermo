using System;

namespace Web.Exceptions
{
    /// <summary>
    /// Excepción base para errores relacionados con la capa de controladores.
    /// </summary>
    public class ControllerException : Exception
    {
        /// <summary>
        /// Inicializa una nueva instancia de <see cref="ControllerException"/> con un mensaje de error.
        /// </summary>
        /// <param name="message">El mensaje que describe el error.</param>
        public ControllerException(string message) : base(message)
        {
        }

        /// <summary>
        /// Inicializa una nueva instancia de <see cref="ControllerException"/> con un mensaje de error y una excepción interna.
        /// </summary>
        /// <param name="message">El mensaje que describe el error.</param>
        /// <param name="innerException">La excepción que es la causa del error actual.</param>
        public ControllerException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

    /// <summary>
    /// Excepción lanzada cuando hay un error en la validación de los datos de entrada en un controlador.
    /// </summary>
    public class ApiValidationException : ControllerException
    {
        /// <summary>
        /// Inicializa una nueva instancia de <see cref="ApiValidationException"/> con un mensaje de error.
        /// </summary>
        /// <param name="message">El mensaje que describe el error de validación.</param>
        public ApiValidationException(string message) : base(message)
        {
        }

        /// <summary>
        /// Inicializa una nueva instancia de <see cref="ApiValidationException"/> con un mensaje de error y una excepción interna.
        /// </summary>
        /// <param name="message">El mensaje que describe el error de validación.</param>
        /// <param name="innerException">La excepción que es la causa del error actual.</param>
        public ApiValidationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

    /// <summary>
    /// Excepción lanzada cuando hay un error en la autenticación o autorización.
    /// </summary>
    public class ApiAuthorizationException : ControllerException
    {
        /// <summary>
        /// Inicializa una nueva instancia de <see cref="ApiAuthorizationException"/> con un mensaje de error.
        /// </summary>
        /// <param name="message">El mensaje que describe el error de autorización.</param>
        public ApiAuthorizationException(string message) : base(message)
        {
        }

        /// <summary>
        /// Inicializa una nueva instancia de <see cref="ApiAuthorizationException"/> con un mensaje de error y una excepción interna.
        /// </summary>
        /// <param name="message">El mensaje que describe el error de autorización.</param>
        /// <param name="innerException">La excepción que es la causa del error actual.</param>
        public ApiAuthorizationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

    /// <summary>
    /// Excepción lanzada cuando ocurre un error durante el procesamiento de los datos recibidos del cliente.
    /// </summary>
    public class RequestProcessingException : ControllerException
    {
        /// <summary>
        /// Inicializa una nueva instancia de <see cref="RequestProcessingException"/> con un mensaje de error.
        /// </summary>
        /// <param name="message">El mensaje que describe el error de procesamiento.</param>
        public RequestProcessingException(string message) : base(message)
        {
        }

        /// <summary>
        /// Inicializa una nueva instancia de <see cref="RequestProcessingException"/> con un mensaje de error y una excepción interna.
        /// </summary>
        /// <param name="message">El mensaje que describe el error de procesamiento.</param>
        /// <param name="innerException">La excepción que es la causa del error actual.</param>
        public RequestProcessingException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

    /// <summary>
    /// Excepción lanzada cuando hay un error en la configuración de la respuesta HTTP.
    /// </summary>
    public class ResponseFormattingException : ControllerException
    {
        /// <summary>
        /// Inicializa una nueva instancia de <see cref="ResponseFormattingException"/> con un mensaje de error.
        /// </summary>
        /// <param name="message">El mensaje que describe el error de formato.</param>
        public ResponseFormattingException(string message) : base(message)
        {
        }

        /// <summary>
        /// Inicializa una nueva instancia de <see cref="ResponseFormattingException"/> con un mensaje de error y una excepción interna.
        /// </summary>
        /// <param name="message">El mensaje que describe el error de formato.</param>
        /// <param name="innerException">La excepción que es la causa del error actual.</param>
        public ResponseFormattingException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

    /// <summary>
    /// Excepción lanzada cuando hay un error relacionado con una operación de integración externa (OAuth, APIs de terceros).
    /// </summary>
    public class ExternalIntegrationException : ControllerException
    {
        /// <summary>
        /// Inicializa una nueva instancia de <see cref="ExternalIntegrationException"/> con un mensaje de error.
        /// </summary>
        /// <param name="message">El mensaje que describe el error de integración.</param>
        public ExternalIntegrationException(string message) : base(message)
        {
        }

        /// <summary>
        /// Inicializa una nueva instancia de <see cref="ExternalIntegrationException"/> con un mensaje de error y una excepción interna.
        /// </summary>
        /// <param name="message">El mensaje que describe el error de integración.</param>
        /// <param name="innerException">La excepción que es la causa del error actual.</param>
        public ExternalIntegrationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
/*
DbOperationException:
Excepcion creada para enviar mensajes relacionados 
con la ejecución de operaciones CRUD en la base de datos
*/

namespace pigmentos.API.Exceptions
{
    public class DbOperationException(string message) : Exception(message)
    {
    }
}
/*
EmptyCollectionException:
Excepcion creada para enviar mensajes relacionados 
con las colecciones o respuestas vacías
*/

namespace pigmentos.API.Exceptions
{
    public class EmptyCollectionException(string message) : Exception(message)
    {
    }
}
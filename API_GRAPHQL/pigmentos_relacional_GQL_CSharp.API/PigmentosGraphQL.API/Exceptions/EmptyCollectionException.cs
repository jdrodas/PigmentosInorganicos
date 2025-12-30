/*
EmptyCollectionException:
Excepcion creada para enviar mensajes relacionados 
con las colecciones o respuestas vacías
*/

namespace PigmentosGraphQL.API.Exceptions
{
    public class EmptyCollectionException(string message) : Exception(message)
    {
    }
}
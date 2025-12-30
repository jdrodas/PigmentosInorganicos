# Pigmentos Inorgánicos

Repositorio del proyecto de gestión de información de Pigmentos Inorgánicos.

APIs REST y GRAPHQL desarrolladas como ejercicio demostrativo para el curso de **Tópicos Avanzados de Bases de Datos**, enfocada
en la implementación del **patrón repositorio** y la separación por capas. El dominio de problema aborda el registro de
la información de pigmentos, colores y familias químicas.

[![Ask DeepWiki](https://deepwiki.com/badge.svg)](https://deepwiki.com/jdrodas/PigmentosInorganicos)
![License](https://img.shields.io/badge/license-Academic-orange)
![.NET](https://img.shields.io/badge/.NET-10.0-purple)
![PostgreSQL](https://img.shields.io/badge/database-postgreSQL-blue)
![MongoDB](https://img.shields.io/badge/database-mongoDB-green)

## Objetivos Académicos

- Demostrar la implementación del **patrón repositorio.**
- Evidenciar la separación clara entre capas de la aplicación.
- Mostrar el desacople de la capa de persistencia para intercambio de bases de datos.
- Implementar versionamiento de APIs siguiendo estándares de la industria.
- Implementar paginación en las respuestas de las peticiones con gran cantidad de resultados.
- Aplicar mejores prácticas de seguridad usando GUIDs en lugar de IDs secuenciales.
- Comparar funcionamiento de API REST y API GraphQL y su desacople de las capas de persistencia de datos.

## Modelo de Datos

### Entidades Principales

#### Pigmentos

- `Id` (UUID): Identificador único del pigmento..
- `nombre` (TEXT): Nombre comercial del pigmento.
- `formula_quimica` (TEXT): Fórmula química del pigmento.
- `numero_ci` (TEXT): Código CI del Pigmento.
- `familia_quimica` (UUID): Identificador único de la familia química.
- `color` (UUID): Identificador único del color principal

#### Colores

- `Id` (UUID): Identificador único del color.
- `nombre` (TEXT): Nombre del color.
- `representacion_hexadecimal` (TEXT): Representación del color para visualización web.

#### Familias Químicas

- `Id` (UUID): Identificador único de la familia.
- `nombre` (TEXT): Nombre de la familia.
- `composicion` (TEXT): detalle composición de la familia.

## Stack Tecnológico

- **Framework base**: C# en .NET 10.x
- **Base de Datos**: PostgreSQL 17.x / MongoDB 8.x
- **ORM**: Dapper (micro-ORM) 2.1.66
- **Documentación**: Swagger/OpenAPI usando Swashbuckle 10.0.1
- **Driver DB Relacional**: Npgsql 10.0.0

## Arquitectura - API REST

### Estructura de Capas

```
Controllers → Services → Repositories (via Interfaces) → DB Context
                  ↓
              IRepositories (Interfaces)
```

### Componentes

- **Controllers**: Capa de presentación y manejo de HTTP.
- **Services**: Lógica de negocio y reglas de dominio.
- **Interfaces**: Contratos para desacoplamiento.
- **Repositories**: Implementaciones de acceso a datos.
- **Models**: Modelos de dominio.
- **DBContext**: Contextos y configuraciones de base de datos.

## Endpoints - API REST

El versionamiento de los endpoints utilizará parámetro en el encabezado o parámetro de consulta, en lugar de incluirlo en el URL

### Pigmentos

```http
GET    /api/pigmentos                       # Listar todos los pigmentos
GET    /api/pigmentos/{id}                  # Obtener por ID
POST   /api/pigmentos                       # Crear nuevo pigmento
PUT    /api/pigmentos                       # Actualizar pigmento
DELETE /api/pigmentos/{id}                  # Eliminar pigmento
```

### Colores

```http
GET    /api/colores                         # Listar todos los colores
GET    /api/colores/{id}                    # Obtener por ID
GET    /api/colores/{id}/pigmentos          # Obtener pigmentos asociados por ID del color
POST   /api/colores                         # Crear nuevo color
PUT    /api/colores                         # Actualizar color
DELETE /api/colores/{id}                    # Eliminar color
```

### Familias

```http
GET    /api/familias                        # Listar todas las familias
GET    /api/familias/{id}                   # Obtener por ID
GET    /api/familias/{id}/pigmentos         # Obtener pigmentos asociados por ID de familia
POST   /api/familias                        # Crear nueva familia
PUT    /api/familias/                       # Actualizar familia
DELETE /api/familias/{id}                   # Eliminar familia
```

# Task Manager

Task Manager es una API que permite a los usuarios gestionar tareas y enlistar tareas pendientes por usuario.

## Herramientas utilizadas

- .NET 10 Web API
- SQL Server 2022 Express
- Entitiy Framework Core

## Especificaciones

### Caso de negocio
Cada tarea contiene:
- Id
- Título
- Descripción
- Prioridad (Alta, Media, Baja)
- Fecha de creación
- Fecha de inicio
- Fecha de finalización
- Fecha límite
- Estatus:
    * Pendiente
    * En progreso
    * Terminada
- Usuario responsable

Para efectos de aplicación el acceso a los endpoints requiere autenticación de usuario. Esta autenticación se implementa mediante token JWT.

### Base de datos
El modelo está construido sobre SQL Server e incluye:
- Scripts de creación
- Llaves primarias
- Llaves foráneas
- Índices donde considere conveniente

Contiene el Stored Procedure sp_GetPendingTasks que regresa:
- Usuario
- Total de tareas pendientes
- Total de tareas vencidas

Contiene el trigger trg_during_update_task que registre en una tabla de auditoría cada que una tarea cambia de
estatus.

### API (.NET)
La base del proyecto es una Web API REST que realiza un CRUD básico en conjunto con EF Core.
#### CRUD de tareas
##### Endpoints
- GET /tasks
- GET /tasks/{id}
- POST /tasks
- PUT /tasks/{id}
- DELETE /tasks/{id}
- GET /reports/pending-tasks

El endpoint GET /tasks incluye los siguientes filtros:
- Prioridad
- Estatus
- Usuario
- Fecha inicial
- Fecha final

El endpoint GET /tasks contiene paginación:
- ?page=1
- &pageSize=20

#### Validaciones
La lógica de negocio incluye las siguientes validaciones:
- No permitir fechas límite menores a hoy.
- El título es obligatorio.
- La descripción máximo 500 caracteres.
- No permitir dos tareas con el mismo título para un mismo usuario.

#### Manejo de errores
La API regresa los siguientes códigos HTTP a través de un Middleware o en el caso del código 200 a través del controlador como:
- 200
- 201
- 400
- 404
- 500

## `ApiResponse<T>`

Plantilla general que estandariza todas las respuestas de la API tanto de éxito como de error utilizado en cada endpoint.

```json
{
  "statusCode": 200,
  "success": true,
  "message": "Éxito",
  "data": { ... },
  "errors": []
}
```

### Propiedades

- StatusCode (int): Código HTTP de la respuesta (200, 201, 400, 401, 404, 500)
- Success (bool) `true` en operaciones exitosas, `false` en cualquier error
- Message (string): Resumen corto y legible del resultado
- Data(T?): Resultado de la operación (`null` cuando hay error)
- Errors (List<string>): Detalle de errores — vacío en respuestas exitosas.
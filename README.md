# Task Manager API

Task Manager API es una API para gestionar tareas colaborativas, diseñada para permitir la autenticación basada en cookies y la administración de roles. Este README describe las principales funcionalidades, configuraciones y cómo empezar a usarla.

## Características

- **Autenticación:** Basada en cookies con rutas protegidas.
- **Administración de Roles:** Soporta roles como `Admin` y `User` para gestionar permisos.
- **Gestor de Tareas:** CRUD para tareas con asignación de usuarios y estados.
- **Validaciones de Seguridad:** Verifica permisos y restricciones según el usuario autenticado.
- **Documentación Swagger:** Incluye un entorno de pruebas interactivo.

## Instalación

### Requisitos Previos

- .NET 6.0 o superior.
- SQL Server para la base de datos.
- Postman (opcional para pruebas manuales).

### Pasos

1. Clona este repositorio:
   ```bash
   git clone https://github.com/tuusuario/task-manager-api.git
   cd task-manager-api
   ```

2. Configura la base de datos y usuario por defecto en el archivo `appsettings.json`:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=tu_servidor;Database=taskmanager;User Id=tu_usuario;Password=tu_password;"
   },
   ```
  ```json
  "DefaultUsername": "usuario_por_defecto",
  "DefaultPassword": "contraseña_por_defecto" 
   ```

3. Aplica las migraciones para inicializar la base de datos:
   ```bash
   dotnet ef database update
   ```

4. Inicia la API:
   ```bash
   dotnet run
   ```

5. Accede a la documentación Swagger en:
   [http://localhost:5000/swagger](http://localhost:5000/swagger)

## Uso

### Autenticación
La autenticación se realiza enviando el `username` y `password` a la ruta:

**Endpoint:**
```http
POST /api/User/Login
```
**Body:**
```json
{
  "username": "user123",
  "password": "mypassword"
}
```

Si la autenticación es exitosa, se devuelve una cookie que debe incluirse en las solicitudes posteriores.

### Operaciones CRUD de Tareas

#### Obtener Todas las Tareas
```http
GET /api/Task
```
**Requiere autenticación.**

#### Crear una Tarea
```http
POST /api/Task
```
**Body:**
```json
{
  "title": "Nueva tarea",
  "description": "Detalles de la tarea",
  "statusId": 1,
  "assignedId": 2
}
```

#### Actualizar una Tarea
```http
PUT /api/Task/{id}
```
**Body:**
```json
{
  "title": "Tarea actualizada",
  "description": "Detalles actualizados",
  "statusId": 2
}
```

#### Eliminar una Tarea
```http
DELETE /api/Task/{id}
```
**Requiere permisos de usuario creador.**

### Administración de Roles
Solo los usuarios con rol `Admin` pueden asignar roles. Ejemplo:

**Endpoint:**
```http
PUT /api/User/{id}
```
**Body:**
```json
{
  "role": "Admin"
}
```

## Configuración de Swagger

La API incluye un esquema de autenticación basado en cookies para probar rutas protegidas desde la interfaz Swagger:

1. Autentícate usando la ruta `/api/User/Login` directamente desde Swagger.
2. Copia la cookie generada y usa las rutas protegidas.

## Contribuciones

1. Realiza un fork de este repositorio.
2. Crea una rama con tu función: `git checkout -b mi-nueva-funcion`.
3. Haz un commit de tus cambios: `git commit -m 'Añade mi nueva función'`.
4. Realiza un push a la rama: `git push origin mi-nueva-funcion`.
5. Abre un Pull Request.

## Licencia

Este proyecto está bajo la licencia MIT. 


## Desafio Tecnico NET - WEB API

En este repositorio les muestro cómo crear una WebAPI con .NET6 que implementa el patrón Clean Code y Mediator.

La WebAPI utiliza la autenticación por medio de token JWT, que es un estándar para transmitir información segura entre partes. El token JWT contiene los datos del usuario y los permisos que tiene para acceder a los recursos de la API.

Por el lado del cliente, se ha mantenido el patrón MVC para separar la lógica de negocio, la interfaz de usuario y la comunicación con el servidor. La APP se comunica con la API mediante peticiones HTTP, enviando y recibiendo datos en formato JSON. Los datos que se envían son los registros de productos y órdenes de clientes.

Para este ejemplo, eh optado por usar <code>Migrations EF Core</code> para mantener la sincronización con la base de datos. Por si tienes dudas, eh dejado el script sql para la importación manual.

Los datos de configuración se encuentran detalladamente en el archivo appsettings de cada proyecto. Algunos datos a tener en cuenta:

### Autenticación de API
```
 Usuario: basicuser@vaetech.net <\br>
 Password: Abc123.
```

### API
Los datos de conexión a la db se encuentra en el archivo appsettings.
```
 Proveedor: MS SQL Server
 Batabase: DesafioTecnicoNET
```

### APP
Los datos de conexión a la api se encuentra en el archivo appsettings.
```
 API_URL(default): https://localhost:44379
```

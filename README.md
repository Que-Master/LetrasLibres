# Documentación DotNet Letras libres

Profesor: Maximiliano Moraga

Fecha: 10/06/2025

Integrantes:  Anibal Tapia, Adrián Ramírez y Luis Romero	


## Pruebas funcionales (Swagger)

### Libro

**Post/api/libro**

**Enviamos los datos al endpoint mediante una solicitud POST para agregar un nuevo libro**

![image](https://github.com/user-attachments/assets/600e4201-22fa-40e0-9b26-15469e34e992)


**Retorno de la consulta**

![image](https://github.com/user-attachments/assets/3a27f284-eb8f-41dc-9453-f68cd1e14f6e)


**GET/api/libro**

**Consulta los libros registrados**

![image](https://github.com/user-attachments/assets/bea5f6bb-3602-4eb2-a8ac-72ed2f3a9c45)


**GET/api/libro/{Id}**

**Consulta los datos del libro buscándolo por su Id**

![image](https://github.com/user-attachments/assets/03c776ea-ed2d-4219-9bf8-33b1953dfd42)


**PUT/api/libro/{Id}**

**Modifica los datos del libro según el su Id**

![image](https://github.com/user-attachments/assets/dbfe0ed0-8d1d-4896-ab63-50be58ef3a6e)


**DELETE/api/libro/{Id}**

**Elimina el libro y sus datos buscándolo por su Id**

![image](https://github.com/user-attachments/assets/22ab4c85-95cf-4391-b82e-1f2f3556fbac)


---

### Usuario

**POST/api/usuario**

**Enviamos los datos al endpoint mediante una solicitud POST para agregar un nuevo usuario**

![image](https://github.com/user-attachments/assets/0c974f6c-5625-415b-94a5-e26b3c3bb677)


**GET/api/usuario**

**Consulta los usuarios registrados**

![image](https://github.com/user-attachments/assets/82a122d9-055c-4187-b558-b812d98fe5f3)


**GET/api/usuario/{Id}**

**Muestra el registro de préstamos según el id del cliente**

![image](https://github.com/user-attachments/assets/6f6144dd-db6a-4cde-882e-37944c38c307)



---

### préstamo

**Post/api/prestamo**

**Solicita la Id de libro y la de usuario para generar un préstamo**

![image](https://github.com/user-attachments/assets/5a5c4ed3-4051-44ac-80a1-632e142bc10c)


**Post/api/devoluciones** 

**Solicita la Id del préstamo para registrar el libro como devuelto**

![image](https://github.com/user-attachments/assets/9870aa87-f182-4755-9a72-338ea51940b6)


---

## validación

**foto del código sin validación**

![image](https://github.com/user-attachments/assets/faa476bb-5571-415f-a82f-1d88d6b94673)


**Libro prestado**

![image](https://github.com/user-attachments/assets/35cb3aad-05dd-4e41-8c2c-fe6b780f1d23)


**Error al intentar borrar un libro prestado**

![image](https://github.com/user-attachments/assets/71afa179-2844-4a4c-90db-77117a520b06)


**Código corregido con validación**

![image](https://github.com/user-attachments/assets/b3bc399e-ac29-4467-bf47-ee7bfafc103d)


**Correcto funcionamiento, no deja eliminar un libro prestado**
![image](https://github.com/user-attachments/assets/e202b5d3-2182-4cef-8e70-bba0a4e7d182)


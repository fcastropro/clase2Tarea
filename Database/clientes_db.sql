CREATE DATABASE IF NOT EXISTS clientes_db CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
USE clientes_db;

CREATE TABLE IF NOT EXISTS clientes (
    id INT AUTO_INCREMENT PRIMARY KEY,
    cedula VARCHAR(10) NOT NULL UNIQUE,
    nombre VARCHAR(80) NOT NULL,
    apellido VARCHAR(80) NOT NULL,
    correo VARCHAR(120) NOT NULL UNIQUE,
    telefono VARCHAR(15) NOT NULL,
    direccion VARCHAR(150) NULL,
    fecha_registro DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
);

INSERT INTO clientes (cedula, nombre, apellido, correo, telefono, direccion)
VALUES
('1800000001', 'Ana', 'Pérez', 'ana.perez@correo.com', '0991111111', 'Ambato'),
('1800000002', 'Luis', 'García', 'luis.garcia@correo.com', '0992222222', 'Quito')
ON DUPLICATE KEY UPDATE nombre = VALUES(nombre);

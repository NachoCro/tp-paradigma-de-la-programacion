# TP 1 - Socios

**Club Social Deportivo** · Aplicación de escritorio Windows Forms en C#

| | |
|---|---|
| **Integrantes** | Fabricio Gullino · Ignacio Crocetti |
| **Lenguaje** | C# / .NET 10 |
| **Interfaz** | Windows Forms |
| **Base de datos** | SQL Server (`TPSocios`) |
| **Modalidad** | Trabajo grupal |

---

## Contenido

- [Qué hace la aplicación](#qué-hace-la-aplicación)
- [Cómo se ejecuta](#cómo-se-ejecuta)
- [Base de datos](#base-de-datos)
- [Pruebas automáticas](#pruebas-automáticas)
- [Estructura del proyecto](#estructura-del-proyecto)
- [Cómo se resuelve cada consigna](#cómo-se-resuelve-cada-consigna)
- [Convenciones de código](#convenciones-de-código)

---

## Qué hace la aplicación

Un único formulario, `frmTPSocios`, que administra los socios del club sobre la
tabla `Socios`. Permite las cuatro operaciones del CRUD:

| Operación | Cómo se hace |
|---|---|
| **Alta** | Cargar los datos y presionar `Registrar` (o `Enter`). |
| **Modificación** | Elegir una fila de la grilla: el formulario se carga con esos datos y el botón pasa a decir `Actualizar`. |
| **Baja** | Elegir una fila y presionar `Supr`, con confirmación previa. |
| **Lectura** | La grilla se completa al abrir la ventana con el listado completo. |

Detalles de la interfaz:

- El botón único de guardado cambia de texto según el modo, de modo que siempre
  queda claro si se va a dar de alta un socio o a pisar los datos de uno existente.
- `Enter` confirma la operación en curso y `Esc` cancela y vuelve al estado inicial.
- `Ctrl+D` alterna entre el tema retro oscuro y el claro.
- Cuando una validación falla, el control causante se sacude y aparece una
  animación de letras; recién después se detalla el motivo del error, para que el
  `MessageBox` no tape la animación.
- Con el foco en un campo de captura, `Supr` sigue borrando el texto y no elimina
  ningún socio.

---

## Cómo se ejecuta

Requisitos: **.NET 10 SDK** y un servidor **SQL Server** accesible.

```bash
# 1. Levantar SQL Server en el puerto 1433 de localhost
docker compose up -d

# 2. Crear la base y la tabla
sqlcmd -S localhost,1433 -U sa -P 'Password123!' -i BaseTPSocios.sql

# 3. Compilar
dotnet build TPSocios/TPSocios.csproj

# 4. Ejecutar
dotnet run --project TPSocios/TPSocios.csproj
```

Para detener el servidor:

```bash
docker compose down
```

### Atajos de teclado

| Tecla | Acción |
|---|---|
| `Enter` | Confirma el alta o la modificación en curso. |
| `Esc` | Cancela la operación y limpia el formulario. |
| `Supr` | Elimina el socio de la fila seleccionada. |
| `Ctrl+D` | Alterna el tema retro. |

---

## Base de datos

- Base: **`TPSocios`**
- Tabla: **`dbo.Socios`**

| Columna | Tipo | Origen en la interfaz |
|---|---|---|
| `IdSocio` | `int IDENTITY` | Oculta en la grilla. |
| `LegajoSocio` | `nvarchar(6)` | `mtxtLegajoSocio` (máscara `?\-####`) |
| `Apellido` | `nvarchar(50)` | `txtApellido` |
| `Nombre` | `nvarchar(50)` | `txtNombre` |
| `Email` | `nvarchar(100)` | `txtEmail` |
| `FechaNacimiento` | `date` | `dtpFechaNacimiento` |
| `CuotaMensual` | `decimal(8,2)` | `txtCuotaMensual` |
| `TipoSocio` | `nvarchar(20)` | `cmbTipoSocio` |
| `Activo` | `bit` | `chkDisponible` |

La cadena de conexión vive en una constante de código,
`Configuracion/CadenaConexion.cs`, y no en un archivo de configuración: para
cambiar servidor o contraseña hay que recompilar. A cambio, el ejecutable no
depende de ningún `.config` externo para conectarse.

---

## Pruebas automáticas

El proyecto incluye su propia batería de pruebas, que se ejecuta con el mismo
ejecutable, sin base de datos:

```bash
dotnet run --project TPSocios/TPSocios.csproj -- --pruebas
```

Cubre los constructores y propiedades de `Socio`, el cálculo de `Edad`, las
reglas de negocio por tipo de socio, la validación del formato de email, la
configuración del formulario (títulos, máscaras, `MaxLength`, columnas de la
grilla, textos del ComboBox), los estados del botón único de guardado, la
apertura en modo alta, los temas retro, la animación de error y la ausencia de
solapamientos entre controles.

El resultado termina con `TODAS LAS PRUEBAS OK` y código de salida `0`.

---

## Estructura del proyecto

```
TPSocios.slnx
docker-compose.yml              SQL Server en el puerto 1433
BaseTPSocios.sql                Crea la base y la tabla con datos de ejemplo
SQLSocios.sql                   Script de la base según la consigna
TP 1 Socios.pdf                 Enunciado del trabajo práctico

TPSocios/
├── TPSocios.csproj
├── Program.cs                  Contenedor de inyección de dependencias
├── Pruebas.cs                  Batería de pruebas automáticas
├── Configuracion/
│   └── CadenaConexion.cs
├── Datos/
│   └── Database.cs             Acceso a SQL Server con ADO.NET
├── Entidades/
│   ├── Socio.cs                Clase de la tabla, propiedades encapsuladas
│   ├── TipoSocio.cs            Enumerado de los tipos de socio
│   └── ItemTipoSocio.cs        Descripción del ComboBox y su valor
├── Repositorios/
│   ├── ISocioRepository.cs     Contrato abstracto
│   ├── SocioRepositorySQL.cs   Implementación funcional
│   └── SocioRepositoryCSV.cs   Implementación sin desarrollar
├── Forms/
│   ├── frmTPSocios.cs          Lógica del formulario
│   └── frmTPSocios.Designer.cs
└── Presentacion/
    ├── Animaciones/
    │   ├── ExplosionTexto.cs
    │   └── Sacudidor.cs
    └── Temas/
        ├── Tema.cs
        ├── Temas.cs
        └── AplicadorTema.cs
```

---

## Cómo se resuelve cada consigna

### Consignas generales

| # | Consigna | Dónde está resuelto |
|---|---|---|
| 1 | Win Forms en C# | Proyecto `TPSocios` |
| 2 | Un único formulario | `Forms/frmTPSocios.cs` |
| 3 | Clase encapsulada con dos constructores | `Entidades/Socio.cs` |
| 4 | Dos repositorios, uno funcional y otro sin implementar | `Repositorios/SocioRepositorySQL.cs`, `Repositorios/SocioRepositoryCSV.cs` |
| 5 | Interfaz | `Repositorios/ISocioRepository.cs` |
| 6 | Inyección de dependencias | `Program.cs:20-22`, constructor de `frmTPSocios` |
| 7 | Validaciones en la capa UI, en dos funciones | `ValidacionFormulario` y `ValidacionReglas` en `frmTPSocios.cs` |
| 8 | Tratamiento de operaciones asíncronas | `await using`, `OpenAsync`, `Task` en `Datos/Database.cs` |
| 9 | `try-catch` | En el formulario y en los repositorios |
| 10 | Compila sin errores | `0 Error(s)`, `0 Warning(s)` |

### Consignas particulares

| # | Consigna | Cómo está resuelta |
|---|---|---|
| 2.1 | `frmTPSocios`, centrado, título `Club Social Deportivo` | Nombre de clase, `StartPosition = CenterScreen` y `Text` |
| 2.2 | Botones `btnRegistrar` y `btnCancelar` | Exactamente esos dos, y ningún otro |
| 2.3 | Grilla de solo lectura, `IdSocio` oculto, títulos espaciados | `ConfigurarGrilla` en `frmTPSocios.cs` |
| 2.4 | Legajo con máscara y único | Máscara `?\-####` y unicidad en `ValidacionReglas` |
| 2.5 | Apellido obligatorio y acotado | `txtApellido` con `MaxLength = 50` |
| 2.6 | Nombre obligatorio y acotado | `txtNombre` con `MaxLength = 50` |
| 2.7 | Email con formato validado, obligatorio y acotado | `txtEmail` con `MaxLength = 100` y `EmailValido` |
| 2.8 | Fecha de nacimiento con tope de edad por tipo | `MaxDate` en el `DateTimePicker` y `CumpleEdadSegunTipo` |
| 2.9 | Cuota mensual mayor a cero | Validación en `ValidacionFormulario` |
| 2.10 | ComboBox de solo selección, primera opción por defecto | `DropDownStyle = DropDownList` y `SelectedIndex = 0` |
| 2.11 | CheckBox `Disponible` | `chkDisponible` |

Opciones del ComboBox, con los textos literales de la consigna:

1. `Menor (< 18 años)`
2. `Mayor (>= 18 y < 60)`
3. `Jubilado (>= 60)`
4. `Familiar (sin restricción)`

---

## Convenciones de código

Siguiendo los apuntes de la cátedra:

- **PascalCase** para clases, propiedades, métodos, constantes y valores de
  enumerados.
- **camelCase** para variables, parámetros y campos.
- Los **campos privados** llevan guion bajo: `_socioRepository`.
- Los **parámetros de constructor** van en `camelCase`.
- Uso de **`this.`** para los miembros de instancia.
- **Llaves** de apertura y cierre en su propia línea.
- Las **interfaces** llevan el prefijo `I`: `ISocioRepository`.
- Los **handlers** de eventos se nombran `Control_Evento`:
  `btnRegistrar_Click`, `frmTPSocios_Load`, `dgvSocios_SelectionChanged`.
- Los **controles** llevan prefijo según su tipo: `btn`, `txt`, `mtxt`, `cmb`,
  `dtp`, `chk`, `lbl`, `dgv`, `gbx`.
- `async`/`await` en los repositorios, con `await using` para los objetos que
  implementan `IAsyncDisposable`.
- `try-catch` en las operaciones que acceden a la base de datos.

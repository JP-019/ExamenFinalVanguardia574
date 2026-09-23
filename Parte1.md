Examen Final — Monolito MVC desde Cero +
Análisis de Migración a N-Capas

n la primera vas a construir, desde cero, un Web API con
patrón Monolito MVC — el mismo patrón que trabajaron con BibliotecaMonolito, pero esta
vez no reciben ningún proyecto de partida: solo un repositorio vacío.

El caso de negocio — TicketExpress
TicketExpress es una plataforma pequeña de venta de boletos para eventos (conciertos,
charlas, partidos). Cualquier persona puede comprar boletos para un evento publicado,
siempre que todavía queden disponibles y el evento no haya pasado.

Modelo de datos
Relación: un Evento tiene muchos Boletos vendidos (1 a N) — el mismo tipo de relación
que Autor–Libro o Cuidador–Mascota.
Entidad Campo Tipo
Evento Id int (autogenerado)
Evento Nombre string
Evento Ciudad string
Evento Fecha DateTime
Evento CapacidadTotal int
Evento PrecioBoleto decimal
Boleto Id int (autogenerado)
Boleto EventoId int (FK a Evento)
Boleto NombreComprador string
Boleto CorreoComprador string
Boleto Cantidad int
Boleto FechaCompra DateTime (se asigna en el
servidor, no la manda el
cliente)

PRIMERA PARTE — Proyecto Monolito MVC

Endpoints requeridos:
Controller Endpoints
EventosController GET (todos), GET por Id, POST, PUT, DELETE
BoletosController GET (todos), GET por Id, POST
Validaciones de formato — Evento:
• Nombre: obligatorio, entre 3 y 100 caracteres.
• Ciudad: obligatoria, entre 2 y 60 caracteres, solo letras, espacios, guiones o
apóstrofes.
• Fecha: obligatoria, no puede ser una fecha ya pasada (debe ser hoy o en el futuro).
• CapacidadTotal: obligatorio, entero mayor a 0.
• PrecioBoleto: obligatorio, no puede ser negativo (0 es válido, para eventos gratuitos).
Validaciones de formato — Boleto:
• NombreComprador: obligatorio, entre 2 y 100 caracteres, solo letras, espacios,
guiones o apóstrofes.
• CorreoComprador: obligatorio, debe tener formato de correo válido.
• Cantidad: obligatorio, entero mayor a 0.
Reglas de negocio (requieren consultar la base de datos, no son solo formato):
1. No se puede crear un Boleto si la Cantidad solicitada supera los boletos disponibles
del evento (CapacidadTotal menos la suma de Cantidad de todos los boletos ya
vendidos para ese Evento)
2. No se puede crear un Boleto para un Evento cuya Fecha ya pasó
3. No se puede eliminar (DELETE) un Evento que ya tiene al menos un Boleto vendido
Funciones de uso común (reutilizables, no atadas a un solo Controller):
• Una función de normalización de texto (recorta espacios, colapsa espacios repetidos,
capitaliza tipo nombre propio) reutilizada en Nombre y Ciudad de Evento, y en
NombreComprador de Boleto — el mismo patrón de TextNormalizer que usaron en
Biblioteca.
• Una función que calcule los boletos disponibles restantes de un Evento
(CapacidadTotal menos los ya vendidos) — reutilizada tanto en la Regla 2 como en
cualquier otro lugar del código donde necesiten ese mismo dato.
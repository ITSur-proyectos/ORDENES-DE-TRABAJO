// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


//function limpiarRtf(rtf) {
//    if (!rtf) return '';

//    // Eliminar las etiquetas RTF usando expresiones regulares
//    let textoLimpio = rtf;

//    // Eliminar las etiquetas RTF principales
//    textoLimpio = textoLimpio.replace(/{\\rtf1[\\s\\S]*?}/g, ''); // Eliminar todo el bloque RTF
//    textoLimpio = textoLimpio.replace(/\\par/g, '\n'); // Reemplazar los saltos de línea
//    textoLimpio = textoLimpio.replace(/\\pard/g, '\n'); // Reemplazar los saltos de párrafo

//    // Limpiar los códigos de formato específicos (tamaño de fuente, tipo de fuente, etc.)
//    textoLimpio = textoLimpio.replace(/\\fs\d+/g, ''); // Eliminar el tamaño de la fuente
//    textoLimpio = textoLimpio.replace(/\\f\d+/g, ''); // Eliminar el tipo de fuente
//    textoLimpio = textoLimpio.replace(/\\ansi/g, ''); // Eliminar la codificación ANSI
//    textoLimpio = textoLimpio.replace(/\\uc\d+/g, ''); // Eliminar los parámetros de codificación Unicode

//    // Eliminar cualquier otro código RTF no necesario
//    textoLimpio = textoLimpio.replace(/\\[^a-zA-Z0-9]/g, ''); // Eliminar cualquier comando RTF no reconocido

//    // Eliminar posibles etiquetas vacías
//    textoLimpio = textoLimpio.replace(/\s+/g, ' ').trim();

//    return textoLimpio;
//}

window.onload = function () {
    const descripcion = document.getElementById('descripcionTexto');

    if (descripcion) {
        // Obtén el texto RTF de la descripción
        const rtfTexto = descripcion.innerText || descripcion.textContent;

        // Muestra el texto RTF en el contenedor
        descripcion.innerHTML = "<pre>" + escapeHTML(rtfTexto) + "</pre>";
    }
};

// Función para escapar los caracteres especiales en HTML
function escapeHTML(text) {
    return text.replace(/[&<>"'`=\/]/g, function (s) {
        return '&#' + s.charCodeAt(0) + ';';
    });
}


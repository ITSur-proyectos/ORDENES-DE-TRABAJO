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


//filtrado de sistema x cliente:
const sistemasPorCliente = {
    "0": [0, 2, 23, 26, 27, 35, 45, 51, 56, 57, 59, 64, 66, 67, 69, 70, 73, 72, 77],
    "1": [73, 65, 68, 64, 59, 57, 43, 53, 51, 45, 35, 34, 40, 41, 27, 26, 23, 17],
    "2": [11, 12, 15, 24, 3, 6, 8, 1, 0, 62],
    "3": [28, 30, 31, 0],
    "4": [0, 4, 25, 33, 46, 80, 91],
    "5": [2],
    "6": [7],
    "7": [2],
    "8": [7],
    "9": [9],
    "11": [10, 2, 26, 51, 22, 35, 61, 64, 59, 73],
    "12": [29, 39, 20, 13, 17, 0],
    "13": [14],
    "15": [16],
    "16": [18],
    "17": [19],
    "18": [21, 2],
    "19": [2, 51, 59, 64, 73],
    "20": [0, 25],
    "21": [2],
    "22": [32],
    "23": [36, 9, 25],
    "24": [25],
    "25": [23, 2, 40],
    "27": [2, 27],
    "28": [25],
    "29": [25, 0],
    "30": [2],
    "31": [25],
    "32": [25],
    "34": [25],
    "35": [25],
    "36": [27, 26, 2, 51, 64, 58, 59, 73, 69],
    "37": [70, 73, 72, 71, 59, 66, 64, 51, 2, 0, 23, 21],
    "38": [25],
    "39": [25],
    "40": [0, 37],
    "41": [2],
    "42": [2, 27, 23, 51, 48, 64, 66, 60, 59, 43, 57, 56, 71, 72, 77, 78, 79, 80, 82, 83, 84, 85, 86, 87, 73, 70, 69, 67, 74, 75, 76, 90, 94, 95, 97, 98],
    "43": [102, 103, 105, 106, 38, 0],
    "44": [42],
    "45": [44],
    "46": [0],
    "47": [25],
    "48": [0, 93, 91],
    "49": [0],
    "50": [0],
    "51": [47],
    "52": [0],
    "53": [0],
    "54": [0],
    "55": [0, 49],
    "56": [0],
    "57": [0],
    "58": [50],
    "59": [52],
    "60": [54],
    "61": [55],
    "62": [50, 0],
    "63": [2, 23, 51, 59, 64, 73],
    "64": [0],
    "65": [63],
    "66": [0],
    "67": [0],
    "68": [0],
    "69": [88, 92, 96],
    "70": [99, 100, 101],
    "71": [0],
    "72": [104]
}


const usuariosPorSistema = {
    "91": [1, 44, 49],
    "95": [11, 12, 28, 40, 44, 45, 49],
    "96": [1, 11, 12, 28, 40, 44, 45, 46, 49],
};

//select SC.Cliente, SC.Sistema from Sistemas S
//inner join Sistemas_Clientes SC on SC.Sistema = S.Sistema
//inner join Clientes C on SC.Cliente = C.Cliente
//order by C.Cliente asc

document.addEventListener("DOMContentLoaded", function () {
    // Adaptamos los select a los nuevos ids:
    const clienteSelect = document.getElementById("ClienteNombre");
    const sistemaSelect = document.getElementById("SistemaNombre");
    const responsableSelect = document.getElementById("Responsable"); // si usás responsable

    if (!clienteSelect || !sistemaSelect) {
        console.error("No se encontraron los select requeridos.");
        return;
    }

    // Funciones que ya tenés definidas en site.js:
    function actualizarSistemas(clienteId) {
        sistemaSelect.innerHTML = "";
        const defaultOption = document.createElement("option");
        defaultOption.value = "-1";
        defaultOption.text = "...";
        sistemaSelect.appendChild(defaultOption);

        let sistemaIds = sistemasPorCliente[clienteId] || Object.keys(nombresSistemas).map(Number);

        sistemaIds.forEach(sid => {
            const sidStr = sid.toString();
            if (nombresSistemas[sidStr]) {
                const option = document.createElement("option");
                option.value = nombresSistemas[sidStr]; // O el valor que uses
                option.text = nombresSistemas[sidStr];
                sistemaSelect.appendChild(option);
            }
        });
    }

    clienteSelect.addEventListener("change", function () {
        // Para usar tu mapping de cliente nombre -> id:
        const clienteNombre = this.value;
        const clienteId = clienteNombreToId[clienteNombre]; // asegurate de tener clienteNombreToId definido

        actualizarSistemas(clienteId);
    });

    // Si ya hay un cliente seleccionado al cargar:
    if (clienteSelect.value && clienteSelect.value !== "-1") {
        const clienteId = clienteNombreToId[clienteSelect.value];
        actualizarSistemas(clienteId);
    }
});


//////////////////////////// vista individual agregar


//document.addEventListener("DOMContentLoaded", function () {
//    const nombresSistemasInput = document.getElementById("nombresSistemas");

//    let nombresSistemas = {};

//    if (nombresSistemasInput?.value) {
//        try {
//            nombresSistemas = JSON.parse(nombresSistemasInput.value);
//        } catch (e) {
//            console.error("Error al parsear nombresSistemas:", e);
//        }
//    }

//    const clienteSelect2 = document.getElementById("cliente"); // minúscula
//    const sistemaSelect2 = document.getElementById("sistema");

//    if (!clienteSelect2 || !sistemaSelect2) {
//        console.warn("No se encontraron los select 'cliente' o 'sistema' para el segundo formulario.");
//        return;
//    }

//    clienteSelect2.addEventListener("change", function () {
//        const clienteId = this.value;
//        sistemaSelect2.innerHTML = "";

//        const defaultOption = document.createElement("option");
//        defaultOption.value = "-1";
//        defaultOption.text = "...";
//        sistemaSelect2.appendChild(defaultOption);

//        const sistemaIds = sistemasPorCliente[clienteId] || Object.keys(nombresSistemas).map(Number);

//        sistemaIds.forEach(sid => {
//            const sidStr = sid.toString();
//            if (nombresSistemas[sidStr]) {
//                const option = document.createElement("option");
//                option.value = sidStr;
//                option.text = nombresSistemas[sidStr];
//                sistemaSelect2.appendChild(option);
//            }
//        });
//    });
//});












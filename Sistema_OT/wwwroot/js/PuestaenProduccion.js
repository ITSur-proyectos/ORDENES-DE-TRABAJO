document.addEventListener("DOMContentLoaded", function () {
    const tipoSelect = document.getElementById("tipoModificacion");
    const detalleTextArea = document.getElementById("modificacionDetalle");

    const formularioInput = document.getElementById("FormulariosModificados");
    const baseDatosInput = document.getElementById("ModificacionesBaseDatos");

    // Inicializar valores actuales desde los input hidden
    let formularioTexto = formularioInput.value || "";
    let baseDatosTexto = baseDatosInput.value || "";

    // Forzar selección por defecto en "formulario" y mostrar su contenido
    tipoSelect.value = "formulario";
    detalleTextArea.value = formularioTexto;

    tipoSelect.addEventListener("change", function () {
        const valorActual = detalleTextArea.value;

        if (this.value === "formulario") {
            baseDatosTexto = valorActual;
            detalleTextArea.value = formularioTexto;
        } else if (this.value === "baseDatos") {
            formularioTexto = valorActual;
            detalleTextArea.value = baseDatosTexto;
        } else {
            detalleTextArea.value = "";
        }
    });

    detalleTextArea.addEventListener("input", function () {
        if (tipoSelect.value === "formulario") {
            formularioTexto = detalleTextArea.value;
        } else if (tipoSelect.value === "baseDatos") {
            baseDatosTexto = detalleTextArea.value;
        }
    });

    const form = detalleTextArea.closest("form");
    form.addEventListener("submit", function () {
        if (tipoSelect.value === "formulario") {
            formularioInput.value = detalleTextArea.value;
        } else if (tipoSelect.value === "baseDatos") {
            baseDatosInput.value = detalleTextArea.value;
        }

        if (!formularioInput.value && formularioTexto) {
            formularioInput.value = formularioTexto;
        }

        if (!baseDatosInput.value && baseDatosTexto) {
            baseDatosInput.value = baseDatosTexto;
        }
    });
});
//function grabarAvances() {
//    const form = document.getElementById('formAvances');
//    const formData = new FormData(form);

//    fetch('/ABMController/GrabarOrden', {
//        method: 'POST',
//        body: formData
//    })
//        .then(response => {
//            if (response.ok) {
//                return response.text(); // o .json() si devolvés JSON
//            } else {
//                throw new Error("Error al grabar avances.");
//            }
//        })
//        .then(data => {
//            alert("Avances grabados correctamente.");
//            // Podés limpiar la tabla o recargar los datos dinámicamente
//        })
//        .catch(error => {
//            alert("Error: " + error.message);
//        });
//}

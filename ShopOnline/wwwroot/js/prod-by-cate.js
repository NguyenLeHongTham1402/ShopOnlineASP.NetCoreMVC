
function updateView(endpoint) {
    fetch(endpoint, {
        method: "PATCH"
    }).then(res => {
        console.info(res);
    })
}

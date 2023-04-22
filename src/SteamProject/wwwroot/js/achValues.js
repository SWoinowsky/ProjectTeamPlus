

document.addEventListener('DOMContentLoaded', () => {

    fetch(`/api/Steam/schema?appId=714010`)
    .then(response => response.json())
    .then(data => console.log(data));
    
    fetch(`/api/Steam/gap?appId=714010`)
    .then(response => response.json())
    .then(data => console.log(data));
})
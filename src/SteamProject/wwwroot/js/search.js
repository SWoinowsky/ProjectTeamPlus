document.addEventListener('DOMContentLoaded', () => {
    const search = document.querySelector('#search-input')
    const container = document.querySelector('.flex-container')
    const games = document.querySelectorAll('.flex-item')

    search.addEventListener('input', () => {
        container.innerHTML = ""
        games.forEach((game) => {
            if (game.id.toUpperCase().includes(search.value.toUpperCase())) {
                container.appendChild(game)
            }
        })
    })
})

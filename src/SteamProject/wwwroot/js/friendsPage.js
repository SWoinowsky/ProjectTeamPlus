document.addEventListener('DOMContentLoaded', () => {
    const userId = document.querySelector('.userId').id
    const steamId = document.querySelector('.steamId').id
    const friendCards = document.querySelectorAll('.friend-card')
    const states = ["offline", "online", "busy", "away", "snooze"]
    const personas = document.querySelectorAll('.friend-persona')
    const games = document.querySelectorAll('.friend-game')
    const search = document.querySelector('#search-input')
    const wrapper = document.querySelector('.wrapper')
    search.addEventListener('input', () => {
        wrapper.innerHTML = ""
        friendCards.forEach((card) => {
            if (card.id.toUpperCase().includes(search.value.toUpperCase())) {
                wrapper.appendChild(card)
            }
        })
    })
    const generateGradient = () => {
        let p1 = `${Math.floor(Math.random() * 100)}%`
        let p2 = `${Math.floor(Math.random() * 100)}%`
        return `radial-gradient(circle at ${p1} ${p2}, rgba(73, 49, 158, 1), rgba(92, 9, 130, 1) 72%)`
    }
    friendCards.forEach((card) => {
        card.style.backgroundImage = generateGradient()
    })
    const fetchStatus = (steamId, userId) => {
        fetch(`/api/Steam/friends?steamid=${steamId}&UserId=${userId}`)
        .then((response) => response.json())
        .then((data) => {
            if (data == null) {
                return
            }
            data.forEach((friend) => {
                personas.forEach((persona) => {
                    if (friend.steamId == persona.id) {
                        persona.innerHTML = states[friend.personaState]
                    }
                })
                games.forEach((game) => {
                    if (friend.steamId == game.id){
                        game.innerHTML = friend.gameExtraInfo
                    }
                })
            })
        })
    }
    fetchStatus(steamId, userId)
    setInterval(() => {
        fetchStatus(steamId, userId)
    }, 60000)
})
document.addEventListener('DOMContentLoaded', () => {
    const userId = document.querySelector('.userId').id
    const steamId = document.querySelector('.steamId').id
    const friendCards = document.querySelectorAll('.friend-card')
    const states = ["offline", "online", "busy", "away", "snooze"]
    const personas = document.querySelectorAll('.friend-persona')
    const games = document.querySelectorAll('.friend-game')
    const search = document.querySelector('#search-input')
    const wrapper = document.querySelector('.wrapper')

    const invBtn = document.querySelector('#send-inv')
    const emailInput = document.querySelector('#email-input')
    const phoneInput = document.querySelector('#phone-input')

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

    const valid = (e) => {
        const exp = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/
        return exp.test(e);
    }



    invBtn.addEventListener('click', () => {
        let e = emailInput.value
        if (valid(e)) {
            fetch(`/api/Steam/sendInvite?email=${e}`, {
                method: 'POST',
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'text/html; charset=utf-8'
                },
            })
            .then(response => {
                if (response.status == 200) {
                    console.log('received 200')
                }
            })
        } else {
            
        }
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
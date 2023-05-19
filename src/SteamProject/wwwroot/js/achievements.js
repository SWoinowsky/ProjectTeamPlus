document.addEventListener('DOMContentLoaded', () => {

    const steamId = document.querySelector('.user-avatar').id
    const games = document.querySelectorAll('.flex-item')
    const buttons = document.querySelectorAll('.achievement-btn')
    const gameImage = document.querySelector('.game-image')
    const loader = document.querySelector('.loading-spinner')
    const container = document.querySelector('#modal-container')
    const error = document.querySelector('.error')
    error.style.display = "none"
    const errorAch = document.querySelector('.error-ach')
    const errorSer = document.querySelector('.error-ser')

    const handleClick = (event) => {
        error.style.display = "none"
        loader.style.display = "flex"
        container.style.display = "none"
        let appId = event.target.id
        let gameImg = document.querySelector('.modal-img')

        gameImg.setAttribute('src', `https://steamcdn-a.akamaihd.net/steam/apps/${appId}/library_600x900_2x.jpg`)
        fetchAchievements(steamId, appId)
    }

    const fetchAchievements = (steamid, appid) => {
        fetch(`/api/Steam/achievements?steamid=${steamid}&appId=${appid}`)
            .then((response) => {
                if (response.status == 204) {
                    throwError(1)
                }
                else {
                    error.style.display = "none"
                    response.json()
                        .then((data) => {
                            let userAch = data
                            fetch(`/api/Steam/schema?appId=${appid}`)
                                .then((response) => response.json())
                                .then((data) => {
                                    let gameSchema = data
                                    generateAchievements(gameSchema, userAch)
                                })
                        })
                }
            })
            .catch((error) => {
                console.error('Error fetching achievements:', error);
            })
    }

    const generateAchievements = (schema, userAch) => {
        const achRow = document.querySelector('#ach-row')
        achRow.innerHTML = ""
        let gameStats = schema.game
        const earnedRatio = document.querySelector('.ach-earned')
        const progress = document.querySelector('.progress-value')
        let count = 0
        if (gameStats == null || gameStats.availableGameStats.achievements == null) {
            container.style.display = "none"
            throwError(0)
        }
        else {
            let schemaArray = schema.game.availableGameStats.achievements
            let achArray = userAch.playerstats.achievements
            let earned = []
            let unearned = []

            const achLookup = achArray.reduce((lookup, ach) => {
                lookup[ach.apiname] = ach.achieved;
                return lookup;
            }, {});

            schemaArray.forEach((sch) => {
                let isEarned = achLookup[sch.name] === 1;
                if (isEarned) {
                    count += 1;
                }

                let achIcon = document.createElement('div')
                achIcon.className = 'col-md-3 ach-col'
                achIcon.innerHTML = `<div class="ach-name">${sch.displayName}</div><img class="ach-img${isEarned ? '' : ' unearned'}" src=${sch.icon}>`

                if (isEarned) {
                    earned.push(achIcon)
                } else {
                    unearned.push(achIcon)
                }
            });

            earnedRatio.textContent = `${count} / ${achArray.length}`
            let percentage = (count / achArray.length) * 200

            loader.style.display = "none"
            container.style.display = "flex"

            earned.forEach((icon) => {
                achRow.appendChild(icon)
            })
            unearned.forEach((icon) => {
                achRow.appendChild(icon)
            })

            progress.style.width = `${percentage}px`
        }
    }



    const throwError = (serverError) => {
        loader.style.display = "none"
        container.style.display = "none"
        error.style.display = "flex"
        if (serverError) {
            errorAch.style.display = "none"
            errorSer.style.display = "flex"
        } else {
            errorAch.style.display = "flex"
            errorSer.style.display = "none"
        }
        return
    }

    buttons.forEach((button) => {
        button.setAttribute('data-toggle', 'modal')
        button.setAttribute('data-target', '#ach-modal')
        button.style.height = "200px"
        button.style.width = "200px"
        button.addEventListener('click', handleClick)
    })
})
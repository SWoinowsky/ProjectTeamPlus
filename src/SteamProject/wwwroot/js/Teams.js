document.addEventListener('DOMContentLoaded', () => {
    let base = location.origin
    let wrapper = document.querySelector('.team-cards')
    let teams = document.querySelectorAll('.team-card')
    let extendos = document.querySelectorAll('.extendo')
    let createBtn = document.querySelector('#send-creation')
    let search = document.querySelector('.search-input')
    let cptState = document.querySelector('.cpt-state').textContent
    let memState = document.querySelector('.mem-state').textContent
    let userId = document.querySelector('.messengerId').innerHTML

    const generateGradient = () => {
        let p1 = `${Math.floor(Math.random() * 100)}%`
        let p2 = `${Math.floor(Math.random() * 100)}%`
        return `radial-gradient(circle at ${p1} ${p2}, rgba(236, 48, 79, 1), rgba(200, 30, 35, 6) 72%)`
    }

    teams.forEach((team) => {
        team.style.backgroundImage = generateGradient()
    })

    extendos.forEach((extendo) => {
        extendo.addEventListener('click', () => {
            let toExt = extendo.parentElement.parentElement.lastElementChild
            if (toExt.classList[1] != "extended") {
                toExt.classList.add('extended')
                extendo.className = "bi bi-chevron-compact-up extendo"
            } else {
                toExt.classList.remove('extended')
                extendo.className = "bi bi-chevron-compact-down extendo"
            }
        })
    })

    teams.forEach((team) => {
        let url = base + `/getMembers?teamId=${team.id}`
        fetch(url)
        .then((response) => response.json())
        .then((data) => {
            let members = data.members
            let memberIcons = document.querySelectorAll(`#i-${team.id}`)
            for (let i = 0; i < members.length; i++) {
                memberIcons[i].src = "../assets/person.fill@2x.png"
            }
            let memberDetails = document.querySelectorAll(`#mem-${team.id}`)
            for (let i = 0; i < members.length; i++) {
                memberDetails[i].querySelector('.member-avi').src = data.members[i].avatar
                memberDetails[i].querySelector('.member-name').textContent = data.members[i].name.slice(0, 7)
                memberDetails[i].querySelector('.member-lvl').textContent = `LVL: ${data.members[i].level}`
                memberDetails[i].querySelector('.member-id').textContent = data.members[i].id
            }

            let newDetails = document.querySelectorAll(`#mem-${team.id}`)
            for (let i = 0; i < newDetails.length; i++) {
                let type = newDetails[i].querySelector('.member-name')
                if ( type.textContent == "Open") {
                    if (cptState == "False" && memState == "False") {
                        memberIcons[i].classList.add("open")
                        memberIcons[i].src = "../assets/person.badge.plus@2x.png"
                        memberIcons[i].addEventListener('click', () => {
                            let url = base + `/addToTeam?teamId=${team.id}&userId=${userId}`
                            fetch(url, 
                                {'method' : 'PATCH',
                                headers : {
                                    'Accept': 'application/json',
                                    'Content-Type': 'text/plain; charset=utf-8'
                                }})
                                .then((response) => {
                                if (response.status == 200) {
                                    location.reload()
                                }
                            })
                        })
                    }
                }
            }

            let memberList = document.querySelectorAll('.member')
            for (let i = 1; i < memberList.length; i++) {
                let memName = memberList[i].querySelector('.member-name').textContent
                if(memName != "Open" && cptState == true) {
                    let id = memberList[i].querySelector('.member-id').textContent

                    memberList[i].classList.add("taken")
                    let ogSrc = memberList[i].querySelector('.member-avi').src
                    let ogName = memberList[i].querySelector('.member-name').textContent
                    memberList[i].addEventListener('mouseover', () => {
                        memberList[i].querySelector('.member-avi').src = "../assets/person.badge.minus@2x.png"
                        memberList[i].querySelector('.member-name').textContent = "Remove"
                    })
                    memberList[i].addEventListener(('mouseleave'), () => {
                        memberList[i].querySelector('.member-avi').src = ogSrc
                        memberList[i].querySelector('.member-name').textContent = ogName
                    })
                    memberList[i].addEventListener('click', () => {
                        let url = base + `/removeFromTeam?userId=${id}`
                        fetch(url, {
                            'method' : 'PATCH',
                            headers : {
                                'Accept': 'application/json',
                                'Content-Type': 'text/plain; charset=utf-8'
                            }
                        })
                        .then(response => {
                            if (response.status == 200) {
                                location.reload()
                            }
                        })
                    })
                }
            }
        })
    })

    createBtn.addEventListener('click', () => {
        let tName = document.querySelector('#name-input')
        let tMotto = document.querySelector('#motto-input')
        let tImg = document.querySelector('#img-input')
        let cId = document.querySelector('.messengerId').textContent
        let url = base + `/createTeam?captain=${cId}&name=${tName.value}&motto=${tMotto.value}&imageUrl=${encodeURIComponent(tImg.value)}`
        fetch(url, {
            'method' : 'POST',
            headers : {
                'Accept': 'application/json',
                'Content-Type': 'text/plain; charset=utf-8'
            }
        })
        .then((response) => {
            if (response.status == 200) {
                document.querySelector('.modal-backdrop').remove();
                location.reload()
            }
        })
    })

    search.addEventListener('input', () => {
        wrapper.innerHTML = ""
        teams.forEach((card) => {
            if (card.className.toUpperCase().includes(search.value.toUpperCase())) {
                wrapper.appendChild(card)
            }
        })
    })

})
document.addEventListener('DOMContentLoaded', () => {

    const app = document.querySelector(".currentApp").id;
    const cards = document.querySelectorAll(".achievementCard");

    const genObj = (sch, gap) =>  {
        schAch = sch.game.availableGameStats.achievements
        gapAch = gap.achievementpercentages.achievements
        let newAch = []
        
        schAch.forEach((sAch) => {
            gapAch.forEach((gAch) => {
                if (sAch.name == gAch.name) {
                    let newObj = {
                        name : sAch.displayName,
                        percentage : Math.round(gAch.percent * 10) / 10 
                    }
                    newAch.push(newObj)
                }
            })
        })
        return newAch
    }

    const assignValues = (achCards, gaps) => {
        achCards.forEach((card) => {
            let achName = card.getElementsByClassName('card-title')[0].textContent

            gaps.forEach((gap) => {
                if (gap.name == achName) {
                    let val = gap.percentage
                    let flare = card.getElementsByClassName('flare')[0]
                    switch (true) {
                        case val >= 90:
                            // iron
                            flare.setAttribute("class", "iron-flare")
                            flare.style.display = "block"
                            card.setAttribute("class", "card achievementCard iron")
                            card.getElementsByClassName('card-body')[0].getElementsByClassName('ach-value')[0].innerText = "1"
                            break;

                        case val >= 75 && val <= 89.9:
                            // gold
                            flare.setAttribute("class", "gold-flare")
                            flare.style.display = "block"
                            card.setAttribute("class", "card achievementCard gold")
                            card.getElementsByClassName('card-body')[0].getElementsByClassName('ach-value')[0].innerText = "3"
                            break;

                        case val >= 60 && val <= 74.9:
                            //plat
                            flare.setAttribute("class", "plat-flare")
                            flare.style.display = "block"
                            card.setAttribute("class", "card achievementCard plat")
                            card.getElementsByClassName('card-body')[0].getElementsByClassName('ach-value')[0].innerText = "5"
                            break;

                        case val >= 35 && val <= 59.9:
                            //diamond
                            flare.setAttribute("class", "diamond-flare")
                            flare.style.display = "block"
                            card.setAttribute("class", "card achievementCard diamond")
                            card.getElementsByClassName('card-body')[0].getElementsByClassName('ach-value')[0].innerText = "7"
                            break;

                        case val >= 1.1 && val <= 34.9:
                            //emerald
                            flare.setAttribute("class", "emerald-flare")
                            flare.style.display = "block"
                            card.setAttribute("class", "card achievementCard emerald")
                            card.getElementsByClassName('card-body')[0].getElementsByClassName('ach-value')[0].innerText = "9"
                            break;

                        case val <= 1:
                            //ruby
                            card.getElementsByClassName('flare')[0]
                            flare.setAttribute("class", "ruby-flare")
                            flare.style.display = "block"
                            card.setAttribute("class", "card achievementCard ruby")
                            card.getElementsByClassName('card-body')[0].getElementsByClassName('ach-value')[0].innerText = "11"
                            break;

                        default:
                            break;
                    }
                }
            })
        })
    }

    async function calcAch() {
        await fetch(`/api/Steam/schema?appId=${app}`)
        .then(response => response.json())
        .then((data) => {
            let schema = data
                fetch(`/api/Steam/gap?appId=${app}`)
                .then(response => response.json())
                .then((data) => {
                    let gap = data
                    gapNew = genObj(schema, gap)
                    assignValues(cards, gapNew)
                })
        })
    }

    calcAch();

})
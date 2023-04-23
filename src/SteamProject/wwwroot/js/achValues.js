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

    const assignValues = (cards, gaps) => {
        cards.forEach((card) => {
            let achName = card.getElementsByClassName('card-title')[0].textContent
            
            card.
            gaps.forEach((gap) => {
                if (gap.name == achName) {
                    let val = gap.percent
                    switch (true) {
                        case val >= 90:
                            // iron
                            card.setAttribute("class", "iron")
                            // card.appendChild("")

                            break;
                        case val >= 75 && val <= 89:
                            // gold
                            break

                        case val >= 60 && val <= 74:
                            //plat
                            break;

                        case val >= 35 && val <= 59:
                            //diamond
                            break;

                        case val >= 6 && val <= 34:
                            //emerald
                            break;

                        case val <= 5:
                            //ruby
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
                })
        })
    }

    calcAch();

})
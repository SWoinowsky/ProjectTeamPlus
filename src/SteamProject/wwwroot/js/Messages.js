document.addEventListener('DOMContentLoaded', () => {
    let messagesBtn = document.querySelector('.messages-btn')
    let messageAlert = document.querySelector('.message-alert')
    let messageContainer = document.querySelector('.message-container')   
    messageContainer.style.visibility = "hidden"
    let messageExit = document.querySelector('#message-exit')
    let messId = document.querySelector('.messengerId').textContent
    let contacts = document.querySelector('.contacts')
    let contactHeader = document.querySelector('.contact-header')
    let messagesBox = document.querySelector('.messages-body')

    const getContacts = () => {
        fetch(`/api/Messages/userContacts?messengerId=${messId}`)
        .then((response) => response.json())
        .then((data) => {
            let r = data.contacts
            r.forEach(contact => {
                contacts.innerHTML += `<div class="contact" id="${contact.id}"><img class="contact-img" src="${contact.avatar}" alt=""></div>`
            });
            let contactBtns = document.querySelectorAll('.contact')
            contactBtns.forEach((contact) => {
                let source = contact.firstChild.src
                contact.addEventListener('click', () => loadHeaderFor(source, name))
            })
        })
    }

    // <div class="contact-label">CONTACTS</div>
    //     <div class="contacts">
    //         <div class="contact" id="69420">
    //             <img class="contact-img" src="~/assets/shortLogo.png" alt="">
    //         </div>
    //     </div>
    // </div>

    // <div class="contact-header">
    //     <img class="specific-contact-img" src="~/assets/shortLogo.png" alt="">
    //     <div class="contact-name">
    //         S.I.N
    //         </div>
    // </div>

    const loadHeaderFor = (source) => {
        // Header
        let cImg = document.createElement('img')
        cImg.className = "specific-contact-img"
        cImg.src = source

        let cName = document.createElement('div')
        cName.className = "contact-name"
        cName.innerText = ""
        contactHeader.appendChild = cImg
        contactHeader.appendChild = cName
    }

    const displayMessages = () => {
        messagesBtn.style.visibility = "hidden"
        messageContainer.style.visibility = "visible"
    }

    const hideMessages= () => {
        messagesBtn.style.visibility = "visible"
        messageContainer.style.visibility = "hidden"
    }

    messageExit.addEventListener('click', () => hideMessages());
    messagesBtn.addEventListener('click', () => displayMessages());

    getContacts(4)
})
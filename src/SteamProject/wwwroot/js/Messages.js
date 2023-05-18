document.addEventListener('DOMContentLoaded', () => {
    let msgsBtn = document.querySelector('.messages-btn')
    let msgAlert = document.querySelector('.message-alert')
    let msgContainer = document.querySelector('.message-container')   
    msgContainer.style.visibility = "hidden"
    let msgExit = document.querySelector('#message-exit')
    let messId = document.querySelector('.messengerId').textContent
    let contacts = document.querySelector('.contacts')
    let contactHeader = document.querySelector('.contact-header')
    let msgContent = document.querySelector('.messages-for-contact')
    let msgTextBox = document.querySelector('.message-text-box')
    let sendBtn = document.querySelector('#send-message-btn')
    var msgCount;

    const initialCount = () => {
        fetch(`/api/Messages/messageCount?messengerId=${messId}`)
        .then(response => response.json())
        .then((data) => {
            msgCount = data;
        })
    }

    const getCount = () => {
        fetch(`/api/Messages/messageCount?messengerId=${messId}`)
        .then(response => response.json())
        .then((data) => {
            if (data > msgCount && msgContainer.style.visibility == "hidden") {
                msgAlert.style.display = "block"
                msgCount = data
            }
        })
    }

    setInterval(() => {
        getCount()
    }, 1000)

    const getContacts = () => {
        fetch(`/api/Messages/userContacts?messengerId=${messId}`)
        .then((response) => response.json())
        .then((data) => {
            let r = data.contacts
            r.forEach(contact => {
                contacts.innerHTML += `<div class="contact ${contact.name} ${contact.nickname}" id="${contact.id}"><img class="contact-img" src="${contact.avatar}" alt=""></div>`
            });
            let contactBtns = document.querySelectorAll('.contact')
            contactBtns.forEach((contact) => {
                if (contact.id == "69420") {
                    contact.addEventListener('click', () => loadContentForSin())
                } else {
                    if (contact.classList[2] != "null") {
                        let avt = contact.firstChild.src
                        let name = contact.classList[2]
                        let id = contact.id
                        contact.addEventListener('click', () => loadContentFor(avt, name, id))
                    } else {
                        let avt = contact.firstChild.src
                        let name = contact.classList[1]
                        let id = contact.id
                        contact.addEventListener('click', () => loadContentFor(avt, name, id))
                    }
                }
            })
        })
    }



    const loadContentForSin = () => {
        contactHeader.innerHTML = `<div class="contact-header"><img class="specific-contact-img" src="../assets/shortLogo.png" alt=""><div class="contact-name">S.I.N</div></div>`
        
    }



    const loadContentFor = (avt, name, id) => {
        contactHeader.innerHTML = ""
        // Header
        let cImg = document.createElement('img')
        cImg.className = "specific-contact-img"
        cImg.src = avt
        contactHeader.innerHTML += `${cImg.outerHTML}`

        let cName = document.createElement('div')
        cName.className = "contact-name"
        cName.innerText = name
        contactHeader.innerHTML += `${cName.outerHTML}`

        msgTextBox.addEventListener('keydown', (event) => {
            if (event.key == "Enter") {
                sendmsg(id, messId, msgTextBox.value)
            }
        })
    
        sendBtn.addEventListener('click', () => {
            sendmsg(id, messId, msgTextBox.value)
        })
    }

    const displayMsgs = () => {
        msgAlert.style.display = "none"
        msgsBtn.style.visibility = "hidden"
        msgContainer.style.visibility = "visible"
    }

    const hideMsgs= () => {
        msgsBtn.style.visibility = "visible"
        msgContainer.style.visibility = "hidden"
    }

    const sendmsg = (to, from, msg) => {
        fetch(`api/Messages/sendMessageTo?toId=${to}&fromId=${from}&message=${msg}`, {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'text/plain; charset=utf-8'
            },
        })
        msgTextBox.value = ""
    }

    msgExit.addEventListener('click', () => hideMsgs());
    msgsBtn.addEventListener('click', () => displayMsgs());

    getContacts(4)
    initialCount()
})
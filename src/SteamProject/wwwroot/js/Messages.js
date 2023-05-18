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
    let msgFooter = document.querySelector('.send-message')
    var msgCount;
    let baseUrl = location.origin;

    const ncode = (s) => {
        let o = s.replace(/<script\b[^<]*(?:(?!<\/script>)<[^<]*)*<\/script>/gi, function(m) {
            return m.replace(/</g, '&lt;').replace(/>/g, '&gt;');
        });
        return o;
    }

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
                msgAlert.style.display = "flex"
                msgCount = data
            }
            if (data > msgCount && msgContainer.style.visibility == "visible") {
                let currentId = document.querySelector('.specific-contact-img').id
                loadContentFor(currentId)
                msgCount = data
            }
        })
    }

    setInterval(() => {
        getCount()
    }, 1000)

    const getContacts = () => {
        let url = baseUrl + `/api/Messages/userContacts?messengerId=${messId}`
        fetch(url)
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
                        contact.addEventListener('click', () => {
                            loadHeaderFor(avt, name, id)
                            loadContentFor(id)
                            loadFooterFor(id)
                        })
                    } else {
                        let avt = contact.firstChild.src
                        let name = contact.classList[1]
                        let id = contact.id
                        contact.addEventListener('click', () => {
                            loadHeaderFor(avt, name, id)
                            loadContentFor(id)
                            loadFooterFor(id)
                        })
                    }
                }
            })
        })
    }

    const loadContentForSin = () => {
        contactHeader.innerHTML = `<div class="contact-header"><img class="specific-contact-img" src="../assets/shortLogo.png" id="69420" alt=""><div class="contact-name">S.I.N</div></div>`
        msgFooter.style.display = "none"
        msgContent.innerHTML = ""
        let url = baseUrl + `/api/Messages/messagesBetween?fromId=69420&toId=${messId}`
        fetch(url)
        .then((response) => response.json())
        .then((data) => {
            let msgs = data.messages
            msgs.forEach((msg) => {
                msgContent.innerHTML += 
                    `
                    <div class="from-message">
                        <div class="from-message-content">
                            ${msg.content}
                        </div>
                        <div class="from-message-time">
                            sent at ${msg.time}
                        </div>
                    </div>
                    `
            })
            msgContent.scrollTop = msgContent.scrollHeight;
        })
    }

    const loadHeaderFor = (avt, name, id) => {
        contactHeader.innerHTML = ""
        // Header
        let cImg = document.createElement('img')
        cImg.className = "specific-contact-img"
        cImg.src = avt
        cImg.id = id
        contactHeader.innerHTML += `${cImg.outerHTML}`

        let cName = document.createElement('div')
        cName.className = "contact-name"
        cName.innerText = name
        contactHeader.innerHTML += `${cName.outerHTML}`
    }

    const loadContentFor = (id) => {
        msgContent.innerHTML = ""
        // Messages
        let url = baseUrl + `/api/Messages/messagesBetween?fromId=${id}&toId=${messId}`
        fetch(url)
        .then((response) => response.json())
        .then((data) => { 
            let msgs = data.messages
            msgs.forEach((msg) => {
                if (msg.from == id) {
                    let msgCnt = document.createElement('div')
                    msgCnt.className = "from-message-content"
                    msgCnt.textContent = msg.content
                    msgContent.innerHTML += `
                    <div class="from-message"> 
                        ${msgCnt.outerHTML}
                        <div class="from-message-time">${msg.time}</div>
                    </div>
                    `
                } else {
                    let msgCnt = document.createElement('div')
                    msgCnt.className = "to-message-content"
                    msgCnt.textContent = msg.content
                    msgContent.innerHTML += `
                    <div class="to-message"> 
                        ${msgCnt.outerHTML}
                        <div class="to-message-time">${msg.time}</div>
                    </div>
                    `
                }
            });
            msgContent.scrollTop = msgContent.scrollHeight;
        })
    }

    const loadFooterFor = (id) => {
        // Message Box
        msgFooter.style.display = "flex"
        msgTextBox.id = id
        toId = id
        msgTextBox.addEventListener('keydown', (event) => {
            if (event.key == "Enter") {
                sendMsg(toId, messId, msgTextBox.value)
            }
        })
        sendBtn.addEventListener('click', () => {
            sendMsg(toId, messId, msgTextBox.value)
        })
    }

    const displayMsgs = () => {
        msgAlert.style.display = "none"
        msgsBtn.style.visibility = "hidden"
        msgContainer.style.visibility = "visible"
        msgFooter.style.display = "none"
    }

    const hideMsgs= () => {
        msgsBtn.style.visibility = "visible"
        msgContainer.style.visibility = "hidden"
    }

    const sendMsg = (to, from, msg) => {
        let url = baseUrl + `/api/Messages/sendMessageTo?toId=${to}&fromId=${from}&message=${msg}`
        fetch(url, {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'text/plain; charset=utf-8'
            },
        })
        .then((data) => {
            if (data.status == 200) {
                msgContent.innerHTML += 
                `
                <div class="to-message">
                    <div class="to-message-content">
                        ${ncode(msg)}
                    </div>
                    <div class="to-message-time">
                        sent at ${new Date().toLocaleString('en-US', { hour: 'numeric', minute: 'numeric', hour12: true })}
                    </div>
                </div>
                `
                msgContent.scrollTop = msgContent.scrollHeight;
            }
        })
        msgTextBox.value = ""        
    }

    msgExit.addEventListener('click', () => hideMsgs());
    msgsBtn.addEventListener('click', () => displayMsgs());

    getContacts()
    initialCount()
})
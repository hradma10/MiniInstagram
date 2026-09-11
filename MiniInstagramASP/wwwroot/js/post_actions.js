async function followUser(event, id, account= false) {
    event.preventDefault();

    const form = event.target;
    const url = form.action;

    const response = await fetch(url, {
        method: "POST",
        headers: {
            "X-Requested-With": "XMLHttpRequest"
        }
    });

    const data = await response.json();

    if (account) {
        const btn = document.querySelector(`[name="follow-btn-${id}"]`);
        if (btn) btn.textContent = data.buttonText;

        const followersElem = document.getElementById(`user-followers-count-${id}`);
        if (followersElem) followersElem.innerText = data.followers;

        const followingElem = document.getElementById(`user-following-count-${id}`);
        if (followingElem) followingElem.innerText = data.following;

    } else {
        const buttons = document.getElementsByName(`follow-btn-${id}`);
        for (const btn of buttons) {
            btn.textContent = data.buttonText;
        }
    }

    return false;
}

async function likePost(event, id) {
    event.preventDefault();

    const form = event.target;
    const url = form.action;

    const response = await fetch(url, {
        method: "POST",
        headers: {
            "X-Requested-With": "XMLHttpRequest"
        }
    });
    
    const spanLike = document.getElementById(`likes-post-${id}`)
    const icon = document.getElementById(`like-icon-${id}`);
    
    const data = await response.json();
    icon.textContent = data.liked ? "❤" : "♡";
    spanLike.textContent = data.likesCount;
    
    return false;
}

async function submitComment(ev, postId) {
    ev.preventDefault();

    const form = ev.target;
    const formData = new FormData(form);

    const url = form.action;

    const response = await fetch(url, {
        method: "POST",
        body: formData,
    });

    document.getElementById(`comments_div-${postId}`).innerHTML = await response.text();
    form.reset();

    return false;
}


async function deleteComment(ev, postId) {
    ev.preventDefault();

    const form = ev.target;
    const formData = new FormData(form);

    const url = form.action;

    const response = await fetch(url, {
        method: "POST",
        body: formData,
    });

    document.getElementById(`comments_div-${postId}`).innerHTML = await response.text();

    return false;
}



async function commentLike(event, commentId) {
    event.preventDefault();

    const form = event.target;
    const url = form.action;

    const response = await fetch(url, {
        method: "POST",
        headers: {
            "X-Requested-With": "XMLHttpRequest"
        }
    });

    const spanLike = document.getElementById(`likes-comment-${commentId}`)
    const icon = document.getElementById(`like-icon-comment-${commentId}`);

    const data = await response.json();
    icon.textContent = data.liked ? "❤" : "♡";
    spanLike.textContent = data.likesCount;
    
    return false;
}
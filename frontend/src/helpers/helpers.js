import moment from "moment";

function getApiDateTime(date) {
    return moment(date).format('YYYY-MM-DDTHH:mm:ss');
}

function getImageUrl(blob) {
    console.log('blob', blob);
    return URL.createObjectURL(blob);
}


function getUniqueString() {
    return '_' + Math.random().toString(36).substr(2, 9);
}

function groupBy(list, keyGetter) {
    const map = new Map();
    list.forEach((item) => {
        const key = keyGetter(item);
        const collection = map.get(key);
        if (!collection) {
            map.set(key, [item]);
        } else {
            collection.push(item);
        }
    });
    return map;
}

function getUserInitials(user) {
    if (!user) {
        return '';
    }
    
    if (user.firstName && user.lastName) {
        return `${user.firstName.charAt(0)}${user.lastName.charAt(0)}`.toUpperCase();
    } else if (user.username) {
        return user.username.slice(0, 1).toUpperCase();
    }
    
    return '';
}

export {
    getApiDateTime,
    getUniqueString,
    groupBy,
    getUserInitials,
    getImageUrl
};

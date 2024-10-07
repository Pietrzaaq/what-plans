﻿function getUniqueString() {
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

export {
    getUniqueString,
    groupBy
};

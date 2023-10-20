/*!
 * Serialize form and object
 * Autor: Luis Eduardo Cochachi Chamorro
 * https://gist.github.com/cochachyLE-Dev/fb828bf38a0d7d353c6de14c652f9a05
 *
 * MIT license
 *
 * Date: 2023-03-15 11:05
 */
function serializeForm(form, tags) {
    let data = '';
    let values = (tags ?? 'input,select').split(',');
    for (let tag of values) {
        Array.from(form.getElementsByTagName(tag))
            .forEach(function (item) {
                if (item.name) {
                    let fields = item.name.split('.');
                    fields.push(item.value);
                    data = [fields].reduce(function (o, n, i) {
                        let j = encodeURI(n[0]);
                        for (let ii in n.slice(1)) {
                            if (ii == 0) continue;
                            j += encodeURI(`[${n[ii]}]`);
                        }
                        j += `=${encodeURI(n[n.length - 1])}`;
                        return `${o}${!o ? '' : '&'}${j}`;
                    }, data);
                }
            });
    }
    return data;
}
function serializeObject(form, tags) {
    let data = {};
    let values = (tags ?? 'input,select').split(',');
    for (let tag of values) {
        Array.from(form.getElementsByTagName(tag)).forEach(function (item) {
            if (item.name) {
                let fields = item.name.split('.');
                fields.push(item.value);
                data = [fields].reduce(function (o, n, i) {
                    let j = {};
                    for (let i in n.slice(1)) {
                        let b = i > n.length - 3;
                        let v = new Function('return ' + `{${n[i]}:${b ? `"${n[n.length - 1]}"` : '{}'}}`)();
                        switch (Number(i)) {
                            case 0:
                                Object.assign(j, v);
                                break;
                            case 1:
                                Object.assign(j[n[0]], v);
                                break;
                            case 2:
                                Object.assign(j[n[0]][n[1]], v);
                                break;
                            case 3:
                                Object.assign(j[n[0]][n[1]][n[2]], v);
                                break;
                            case 4:
                                Object.assign(j[n[0]][n[1]][n[2]][n[3]], v);
                                break;
                            case 5:
                                Object.assign(j[n[0]][n[1]][n[2]][n[3]][n[4]], v);
                                break;
                            case 6:
                                Object.assign(j[n[0]][n[1]][n[2]][n[3]][n[4]][n[5]], v);
                                break;
                        }
                    }
                    switch (n.length - 1) {
                        case 1:
                            o[n[0]] = j[n[0]];
                            break;
                        case 2:
                            o[n[0]] ??= {};
                            Object.assign(o[n[0]], j[n[0]]);
                            break;
                        case 3:
                            o[n[0]] ??= {}, o[n[0]][n[1]] ??= {};
                            Object.assign(o[n[0]][n[1]], j[n[0]][n[1]]);
                            break;
                        case 4:
                            o[n[0]] ??= {}, o[n[0]][n[1]] ??= {}, o[n[0]][n[1]][n[2]] ??= {};
                            Object.assign(o[n[0]][n[1]][n[2]], j[n[0]][n[1]][n[2]]);
                            break;
                        case 5:
                            o[n[0]] ??= {}, o[n[0]][n[1]] ??= {}, o[n[0]][n[1]][n[2]] ??= {}, o[n[0]][n[1]][n[2]][n[3]] ??= {};
                            Object.assign(o[n[0]][n[1]][n[2]][n[3]], j[n[0]][n[1]][n[2]][n[3]]);
                            break;
                        case 6:
                            o[n[0]] ??= {}, o[n[0]][n[1]] ??= {}, o[n[0]][n[1]][n[2]] ??= {}, o[n[0]][n[1]][n[2]][n[3]][n[4]] ??= {};
                            Object.assign(o[n[0]][n[1]][n[2]][n[3]][n[4]], j[n[0]][n[1]][n[2]][n[3]][n[4]]);
                            break;
                    }
                    return data;
                }, data);
            }
        });
    }
    return data;
};

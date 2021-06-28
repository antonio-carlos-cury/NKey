function CSharpListToArray(strList)
{
    return strList.replace(/&quot;/g, '').replace('[', '').replace(']', '').split(',')
}

function Animate(jObject) {
    jObject.animate({ height: "toggle", opacity: "toggle" }, "slow")
}
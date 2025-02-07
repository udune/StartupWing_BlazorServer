window.TodayHoliday_Show = () => showTooltip("todayHoliday_tooltip")
window.TodayHoliday_Hide = () => hideTooltip("todayHoliday_tooltip")

window.LegalEducation_Show_0 = () => showTooltip("legalEducation_tooltip_first");
window.LegalEducation_Hide_0 = () => hideTooltip("legalEducation_tooltip_first");

window.LegalEducation_Show_1 = () => showTooltip("legalEducation_tooltip_second");
window.LegalEducation_Hide_1 = () => hideTooltip("legalEducation_tooltip_second");

function showTooltip(id) {
    let table = document.getElementById(id);
    table.style.display = "block";

    const tooltipWidth = table.offsetWidth;
    const tooltipHeight = table.offsetHeight;

    function followMouse(event) {
        let posX = event.pageX + 10;
        let posY = event.pageY + 10;

        if (posX + tooltipWidth > window.innerWidth) {
            posX = event.pageX - tooltipWidth - 10;
        }
        if (posY + tooltipHeight > window.innerHeight) {
            posY = event.pageY - tooltipHeight - 10;
        }

        table.style.left = posX + "px";
        table.style.top = posY + "px";
    }

    document.addEventListener("mousemove", followMouse);

    table.addEventListener("mouseout", () => {
        table.style.display = "none";
        document.removeEventListener("mousemove", followMouse);
    });
}

function hideTooltip(id) {
    let table = document.getElementById(id);
    table.style.display = "none";
}
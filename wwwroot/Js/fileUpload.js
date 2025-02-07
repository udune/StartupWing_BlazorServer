export function startFileDropZone(dropZoneElement, inputFile) {

    function onDragHover(e) {
        e.preventDefault();
        dropZoneElement.classList.add("hover");

    }

    function onDragLeave(e) {
        e.preventDefault();
        dropZoneElement.classList.remove("hover");

    }

    function onDrop(e) {
        e.preventDefault();
        dropZoneElement.classList.remove("hover");

        inputFile.files = e.dataTransfer.files;
        const event = new Event('change', { bubbles: true });
        inputFile.dispatchEvent(event);

    }

    function onPaste(e) {
        e.preventDefault();
        dropZoneElement.classList.remove("hover");

        inputFile.files = e.clipboardData.files;
        const event = new Event('change', { bubbles: true });
        inputFile.dispatchEvent(event);

    }

    dropZoneElement.addEvnetListener("dragenter", onDragHover);
    dropZoneElement.addEvnetListener("dragover", onDragHover);
    dropZoneElement.addEvnetListener("dragleav", onDragLeave);
    dropZoneElement.addEvnetListener("drop", onDrop);
    dropZoneElement.addEvnetListener("paste", onPaste);

    return {
        dispose: () => {
            dropZoneElement.addEvnetListener("dragenter", onDragHover);
            dropZoneElement.addEvnetListener("dragover", onDragHover);
            dropZoneElement.addEvnetListener("dragleav", onDragLeave);
            dropZoneElement.addEvnetListener("drop", onDrop);
            dropZoneElement.addEvnetListener("paste", onPaste);
        }
    }
}
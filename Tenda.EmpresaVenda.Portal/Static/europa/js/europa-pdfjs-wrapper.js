Europa.Components.PdfViewer = function (dataUrl, elementId, pdfJsViewer) {
    var options = {
        forcePDFJS: true,
        PDFJS_URL: pdfJsViewer
    };
    PDFObject.embed(dataUrl, elementId, options);
};
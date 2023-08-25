// ------------------------------------------------------------------------------------------------------------------ //
//
// Exibir a largura em pixels na console do navegador e no HTML com id="output3" e no rodapé do navbar
//
// ------------------------------------------------------------------------------------------------------------------ //


// Obter a referência para o elemento <html>
const htmlElement = document.getElementsByTagName('html')[0];

// Obter a largura do elemento <html> em pixels
const htmlWidth = htmlElement.clientWidth;

// Obter a altura do elemento <html> em pixels
const htmlHeight = htmlElement.clientHeight;


// -------------------------------------------------------- //
// Exibir a largura em pixels no console
console.log('Largura do <html>: ' + htmlWidth + 'px');

// Exibir a altura em pixels no console
console.log('Altura do <html>: ' + htmlHeight + 'px');

// Exibir a altura em pixels no console
console.log(htmlWidth + 'px X ' + htmlHeight + 'px');


// -------------------------------------------------------- //
// Exibir a largura em pixels no rodapé - htmlWidth
const htmlWidthElement = document.getElementById('htmlWidth');
if (htmlWidthElement) {
    htmlWidthElement.textContent = 'Largura do <html>: ' + htmlWidth + 'px';
}

// Exibir a altura em pixels no rodapé - htmlHeight
const htmlHeightElement = document.getElementById('htmlHeight');
if (htmlHeightElement) {
    htmlHeightElement.textContent = 'Altura do <html>: ' + htmlHeight + 'px';
}

// Exibir a altura em pixels no rodapé do navbar - htmlWidthHeight-navbar
const htmlWidthHeightElement = document.getElementById('htmlWidthHeight');
if (htmlWidthHeightElement) {
    htmlWidthHeightElement.textContent = htmlWidth + 'px X ' + htmlHeight + 'px';
}


// -------------------------------------------------------- //
// Exibir a largura em pixels no rodapé do navbar - htmlWidth-navbar
const htmlWidthNavbarElement = document.getElementById('htmlWidth-navbar');
if (htmlWidthNavbarElement) {
    htmlWidthNavbarElement.textContent = 'Largura do <html>: ' + htmlWidth + 'px';
}

// Exibir a altura em pixels no rodapé do navbar - htmlHeight-navbar
const htmlHeightNavbarElement = document.getElementById('htmlHeight-navbar');
if (htmlHeightNavbarElement) {
    htmlHeightNavbarElement.textContent = 'Altura do <html>: ' + htmlHeight + 'px';
}

// Exibir a altura em pixels no rodapé do navbar - htmlWidthHeight-navbar
const htmlWidthHeightNavbarElement = document.getElementById('htmlWidthHeight-navbar');
if (htmlWidthHeightNavbarElement) {
    htmlWidthHeightNavbarElement.textContent = htmlWidth + 'px X ' + htmlHeight + 'px';
}

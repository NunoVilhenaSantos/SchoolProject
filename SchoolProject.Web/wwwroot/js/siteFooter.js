// ------------------------------------------------------------------------------------------------------------------ //
//
// Exibir a largura em pixels na console do navegador e no HTML com id="output3" e no rodapé do navbar
//
// ------------------------------------------------------------------------------------------------------------------ //


// Obter a referência para o elemento <html>
const htmlElement = document.getElementsByTagName('html')[0];

// Obter a largura do elemento <html> em pixels
const htmlWidth = htmlElement.clientWidth;

// Exibir a largura em pixels no console
console.log('Largura do <html>: ' + htmlWidth + 'px');

// Exibir a largura em pixels no rodapé - output3
const output3Element = document.getElementById('output3');
if (output3Element) {
    output3Element.textContent = 'Largura do <html>: ' + htmlWidth + 'px';
}

// Exibir a largura em pixels no rodapé do navbar - output3-navbar
const output3NavbarElement = document.getElementById('output3-navbar');
if (output3NavbarElement) {
    output3NavbarElement.textContent = 'Largura do <html>: ' + htmlWidth + 'px';
}
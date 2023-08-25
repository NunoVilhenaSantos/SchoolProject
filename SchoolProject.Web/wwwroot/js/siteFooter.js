#!charset=utf-8;
// Content-Type: text/javascript; charset=utf-8

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


// Função para atualizar os elementos da classe personalizada
function updateCustomFooterElements(width, height) {
    const customFooterElements = document.querySelectorAll('.my-custom-footer-html');

    customFooterElements.forEach(element => {
        const widthElement = element.querySelector('.html-width');
        const heightElement = element.querySelector('.html-height');
        const widthHeightElement = element.querySelector('.html-width-height');

        if (widthElement) {
            widthElement.textContent = 'Largura do <html>: ' + width + 'px';
        }

        if (heightElement) {
            heightElement.textContent = 'Altura do <html>: ' + height + 'px';
        }

        if (widthHeightElement) {
            widthHeightElement.textContent = width + 'px X ' + height + 'px';
        }
    });
}


// Chamar a função para atualizar os elementos da classe personalizada
updateCustomFooterElements(htmlWidth, htmlHeight);

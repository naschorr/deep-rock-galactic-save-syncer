// todo: proper coordinate struct

function getMidpoint(coordinatesA, coordinatesB) {
    return {
        x: (coordinatesA.x + coordinatesB.x) / 2,
        y: (coordinatesA.y + coordinatesB.y) / 2
    }
}

function getCentroidOfElement(element) {
    const clientRect = element.getBoundingClientRect();

    return {
        x: clientRect.x + clientRect.width / 2,
        y: clientRect.y + clientRect.height / 2
    }
}

/**
 * Places the element with id `targetElementId` between the elements with ids `elementIdA` and `elementIdB` at the
 * midpoint. Can optionally keep doing this when the window is resized.
 * @param {string} targetElementId The element to be moved
 * @param {string} elementIdA One of the elements the target element will be placed between
 * @param {string} elementIdB One of the elements the target element will be placed between
 * @param {boolean} doOnce If true, perform this placement once, and do not listen for resize events
 */
function placeElementBetweenElements(targetElementId, elementIdA, elementIdB, doOnce) {
    const targetElement = document.getElementById(targetElementId);
    const elementA = document.getElementById(elementIdA);
    const elementB = document.getElementById(elementIdB);

    if (!targetElement) {
        throw new DOMException(`Unable to find element with id: ${targetElementId}`);
    }
    if (!elementA) {
        throw new DOMException(`Unable to find element with id: ${elementAId}`);
    }
    if (!elementB) {
        throw new DOMException(`Unable to find element with id: ${elementBId}`);
    }

    // The coordinates of the midpoint of the line between the centroid of elements A and B
    const targetCoordinates = getMidpoint(getCentroidOfElement(elementA), getCentroidOfElement(elementB))

    const targetElementClientRect = targetElement.getBoundingClientRect();

    // todo: iron out these offsets
    var topOffset = targetCoordinates.y - targetElementClientRect.height / 2;
    var leftOffset = targetCoordinates.x - targetElementClientRect.width - 2;

    targetElement.style.top = `${topOffset}px`;
    targetElement.style.left = `${leftOffset}px`;

    if (!doOnce) {
        window.addEventListener('resize', () => placeElementBetweenElements(targetElementId, elementIdA, elementIdB, true));
    }
}
function scrollFocus(elementId) {
    var $this = $('#elementId'),
        $focusElement = elementId,

  //$('html, body').animate({
  //  scrollTop: $($toElement).offset().top + $offset
  //}, $speed);
  
  if ($focusElement) $($focusElement).focus();
}
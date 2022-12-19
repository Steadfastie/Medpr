import { animation, trigger, animateChild, group,
  transition, animate, style, query, keyframes, sequence, stagger } from "@angular/animations";

export const inAnimation = animation([
  query(':enter', [
    style({height: '0', opacity: 0, 'padding-top': '0', 'padding-bottom': '0'}),

    stagger(50,[
      animate('500ms ease-in-out', style({height: '*', 'padding-top': '*', 'padding-bottom': '*'})),
      animate('300ms ease-in-out', style({'opacity': '1'}))
    ])
  ], { optional: true })
]);

// export const outAnimation = animation([
//   query(':leave', [
//     style({opacity: 1, height: '*', 'padding-top': '*', 'padding-bottom': '*', width:'*', 'padding-left': '*', 'padding-right': '*'}),
//     sequence([
//       animate('300ms ease-in-out', style({opacity: '0'})),
//       animate('300ms ease-in-out', style({height: '0', 'padding-top': "0", 'padding-bottom': "0", width:'*', 'padding-left': '*', 'padding-rigth': '*'}))
//     ])
//   ], { optional: true })
// ]);

import { trace } from '@opentelemetry/api';

export const dynamic = 'force-dynamic';

async function getApiHealth() {
    return await trace
        .getTracer('halcyon-web')
        .startActiveSpan('getApiHealth', async (span) => {
            try {
                const response = await fetch(
                    `${process.env.services__api__https__0}/health`
                );
                return await response.text();
            } finally {
                span.end();
            }
        });
}

export default async function Home() {
    const health = await getApiHealth();

    return (
        <>
            <div className="mx-auto max-w-screen-sm space-y-6 p-6 md:p-10">
                <h1 className="scroll-m-20 text-4xl font-extrabold tracking-tight lg:text-5xl">
                    Welcome!
                </h1>

                <p className="leading-7">
                    Lorem ipsum dolor sit amet, consectetur adipiscing elit.
                    Etiam semper diam at erat pulvinar, at pulvinar felis
                    blandit. Vestibulum volutpat tellus diam, consequat gravida
                    libero rhoncus ut. Morbi maximus, leo sit amet vehicula
                    eleifend, nunc dui porta orci, quis semper odio felis ut
                    quam.
                </p>

                <h2 className="scroll-m-20 border-b pb-2 text-3xl font-semibold tracking-tight">
                    Fusce condimentum
                </h2>

                <p className="leading-7">
                    Fusce vitae commodo metus. Pellentesque a eleifend dolor.
                    Morbi et finibus elit, accumsan sodales turpis. Nulla
                    bibendum pulvinar enim vitae malesuada. Nullam nulla justo,
                    ullamcorper et dui vel, pulvinar mattis enim. Ut dignissim
                    laoreet neque, eget placerat nisl auctor ac. Quisque id quam
                    sollicitudin, suscipit dui a, tempus justo. Aliquam iaculis
                    nisl lacus, non accumsan velit facilisis sed. Nulla commodo
                    sapien sit amet mauris sollicitudin, in lobortis quam
                    lacinia. Donec at pharetra neque, in accumsan dolor.
                </p>
                
                <h2 className="scroll-m-20 border-b pb-2 text-3xl font-semibold tracking-tight">
                    Morbi venenatis
                </h2>

                <p className="leading-7">
                    Morbi venenatis, felis ut cursus volutpat, dolor tortor
                    pulvinar nisl, ac scelerisque quam tortor sit amet ante.
                    Aliquam feugiat nisl arcu, sit amet tincidunt erat tempus
                    ut. Quisque laoreet purus et tempor dignissim. Phasellus
                    vehicula dapibus quam eget faucibus. Sed non posuere lorem.
                    Mauris sit amet risus imperdiet, scelerisque velit at,
                    condimentum nisl. Integer at ligula nisl. Donec sodales
                    justo mi, et bibendum enim bibendum quis. Vestibulum non
                    magna auctor massa efficitur maximus.
                </p>
            </div>
        </>
    );
}

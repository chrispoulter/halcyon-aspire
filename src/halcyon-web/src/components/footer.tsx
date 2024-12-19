export function Footer() {
    return (
        <footer className="border-t">
            <div className="mx-auto flex max-w-screen-sm items-center justify-between gap-2 text-balance px-6 py-2 text-center text-sm text-muted-foreground">
                <span>
                    &copy;{' '}
                    <a
                        href="http://www.chrispoulter.com"
                        className="underline underline-offset-4 hover:text-primary"
                    >
                        Chris Poulter
                    </a>{' '}
                    2024
                </span>
                <span>v1.0.0</span>
            </div>
        </footer>
    );
}
